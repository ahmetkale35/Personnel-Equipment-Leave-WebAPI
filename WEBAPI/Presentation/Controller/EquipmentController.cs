using AutoMapper;
using Entities.DataTransferObject.EquipmentDTO;
using Entities.Exceptions.EquipmentExceptions;
using Entities.Exceptions.UserExceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System.Security.Claims;

namespace Presentation.Controller
{
    
    [ServiceFilter(typeof(LogFilterAttribute))]
    [ApiController]
    [Route("api/[controller]")]

    public class EquipmentController : ControllerBase
    {
        private readonly IServiceManager _manager;
        public readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public EquipmentController(IServiceManager manager, UserManager<User> userManager, IMapper mapper)
        {
            _manager = manager;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllEquipments")]
        public IActionResult GetAllEquipments([FromQuery]EquipmentParameters equipmentParameters)
        {
            var equipments = _manager.Equipment.GetAllEquipments(equipmentParameters ,false);
            return Ok(equipments);
        }

        [HttpGet]
        [Route("GetAllApprovedEquipments")]
        public IActionResult GetAllApprovedEquipments()
        {
            var equipments = _manager.Equipment.GetAllApprovedEquipments(false);
            return Ok(equipments);
        }

        [HttpGet]
        [Route("GetAllStocks")]

        public IActionResult GetAllStocks()
        {
            var stocks = _manager.Equipment.GetAllStocks(false);
            return Ok(stocks);
        }

        [Authorize]
        [HttpGet]
        [Route("GetOneEquipment/{id:int}")]
        public IActionResult GetOneEquipment([FromRoute(Name = "id")] int id)
        {
            var equipment = _manager.Equipment.GetOneEquipmentByIDWithRelations(id, false);
            if (equipment is null)
                throw new EquipmentNotFoundException(id);

            return Ok(equipment);
        }

        [ServiceFilter(typeof(ValidationActionAttribute))]
        [Authorize]
        [HttpPost]
        [Route("CreateOneEquipment")]
        public IActionResult CreateOneEquipment([FromBody] EquipmentDtoInsertion equipmentDtoInsertion)
        {

            if (!_manager.Equipment.IsEquipmentExists(equipmentDtoInsertion.EquipmentItemId, false))
                throw new EquipmentOutOfStockException(equipmentDtoInsertion.EquipmentItemId, _manager.Equipment.StockCount(equipmentDtoInsertion.EquipmentItemId,false));

            int stockCount = _manager.Equipment.StockCount(equipmentDtoInsertion.EquipmentItemId, false);

            if (stockCount < equipmentDtoInsertion.Adet)
            throw new InsufficientEquipmentStockException(equipmentDtoInsertion.EquipmentItemId);

            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return Unauthorized();
            var user = _userManager.FindByNameAsync(username).Result;
            if (user == null)
                return Unauthorized();

            equipmentDtoInsertion.UserId = user.Id;
            var createdEquipment = _manager.Equipment.CreateOneEquipment(equipmentDtoInsertion);
            return StatusCode(201, createdEquipment); // CreatedAtRoute()
        }

        [ServiceFilter(typeof(ValidationActionAttribute))]
        [Authorize]
        [HttpPut]
        [Route("UpdateOneEquipment/{id:int}")]
        public IActionResult UpdateOneEquipment([FromRoute(Name = "id")] int id,
            [FromBody] EquipmentDtoForUpdate equipmentDtoForUpdate)
        {
            _manager.Equipment.UpdateOneEquipment(id, equipmentDtoForUpdate, false);
            return NoContent(); // 204
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteOneEquipment/{id:int}")]
        public IActionResult DeleteOneEquipment([FromRoute(Name = "id")] int id)
        {
            _manager.Equipment.DeleteOneEquipment(id, false);
            return NoContent(); // 204

        }

        [Authorize(Roles = "Employee,It,Admin")]
        [HttpGet]
        [Route("MyRequests")]
        public IActionResult MyEquipments()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return Unauthorized();
            var user = _userManager.FindByNameAsync(username).Result;
            if (user == null)
                return Unauthorized();
            var myEquipments = _manager.Equipment.MyEquipments(user.Id, false);
            if (!myEquipments.Any())
                throw new UserHasNoEquipmentRequestsException(user.Id);
            return Ok(myEquipments);
        }

        [Authorize(Roles = "Admin,It")]
        [HttpGet]
        [Route("Pending")]
        public IActionResult Pending()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return Unauthorized();
            var user = _userManager.FindByNameAsync(username).Result;
            if (user == null)
                return Unauthorized();
            var pendingEquipments = _manager.Equipment.Pending(user.Id, false);
            
            if (!pendingEquipments.Any())
                throw new UserHasNoEquipmentRequestsException(user.Id);
            return Ok(pendingEquipments);
        }

        [Authorize(Roles = "Admin,It")]
        [HttpPut]
        [Route("{id:int}/approve")]
        public IActionResult Approve([FromRoute(Name = "id")] int id)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return Unauthorized();
            var user = _userManager.FindByNameAsync(username).Result;
            if (user == null)
                return Unauthorized();

            var equipmentRequest = _manager.Equipment.GetOneEquipmentByIDWithRelations(id, false);
            if (equipmentRequest == null)
                throw new EquipmentNotFoundException(id);

            if (equipmentRequest.Durum != "Bekliyor")
                throw new EquipmentAlreadyProcessedException(id, equipmentRequest.Durum);

            var equipmentId = equipmentRequest.EquipmentItemId;
            var stockCount = _manager.Equipment.StockCount(equipmentId, false);

            
            if (!_manager.Equipment.IsEquipmentExists(equipmentId, false))
                throw new EquipmentOutOfStockException(equipmentId, stockCount);

            _manager.Equipment.CheckEquipmentStock(equipmentRequest.Id, false);

            equipmentRequest = _manager.Equipment.DecreaseCount(equipmentRequest.Id, false); // Stoktan düş
            equipmentRequest.Durum = "Onaylandı";

            equipmentRequest.OnaylayanId = user.Id;
            var equipmentDtoforUpdate = _mapper.Map<EquipmentDtoForUpdate>(equipmentRequest);
            _manager.Equipment.UpdateOneEquipment(id, equipmentDtoforUpdate, false);
            return Ok(equipmentRequest);
            
        }

        [Authorize(Roles = "Admin,It")]
        [HttpPut("{id:int}/reject")]
        public IActionResult Reject([FromRoute] int id)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return Unauthorized();
            var user = _userManager.FindByNameAsync(username).Result;
            if (user == null)
                return Unauthorized();

            var equipment = _manager.Equipment.GetOneEquipmentByIDWithRelations(id, false);

            if (equipment == null)
                throw new EquipmentNotFoundException(id);


            if (equipment.Durum != "Bekliyor")
                throw new EquipmentAlreadyProcessedException(id, equipment.Durum);

            equipment.Durum = "Reddedildi";
            equipment.OnaylayanId = user.Id;
            _manager.Equipment.UpdateOneEquipment(id, _mapper.Map<EquipmentDtoForUpdate>(equipment), false);

            return Ok(equipment);
        }


    }
}