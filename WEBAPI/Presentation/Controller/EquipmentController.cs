using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Entities.DataTransferObject.EquipmentDTO;
using System.Security.Claims;
using AutoMapper;
using Entities.Exceptions.UserExceptions;
using Entities.Exceptions.EquipmentExceptions;

namespace Presentation.Controller
{
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
        public IActionResult GetAllEquipments()
        {
            var equipments = _manager.Equipment.GetAllEquipmentsWithRelations(false);
            return Ok(equipments);
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

        [Authorize]
        [HttpPost]
        [Route("CreateOneEquipment")]
        public IActionResult CreateOneEquipment([FromBody] EquipmentDtoInsertion equipmentDtoInsertion)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return Unauthorized();
            var user = _userManager.FindByNameAsync(username).Result;
            if (user == null)
                return Unauthorized();

            if (equipmentDtoInsertion == null)
                return BadRequest("Equipment data is null");

            equipmentDtoInsertion.UserId = user.Id;

            var createdEquipment = _manager.Equipment.CreateOneEquipment(equipmentDtoInsertion);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState); // 422

            return StatusCode(201, createdEquipment); // CreatedAtRoute()
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateOneEquipment/{id:int}")]
        public IActionResult UpdateOneEquipment([FromRoute(Name = "id")] int id,
            [FromBody] EquipmentDtoForUpdate equipmentDtoForUpdate)
        {
            if (equipmentDtoForUpdate == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState); // 422

            _manager.Equipment.UpdateOneEquipment(id, equipmentDtoForUpdate, false);
            return NoContent(); // 204
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteOneEquipment/{id:int}")]
        public IActionResult DeleteOneEquipment([FromRoute(Name = "id")] int id)
        {
            _manager.Equipment.DeleteOneEquipment(id, true);
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

            var equipment = _manager.Equipment.GetOneEquipmentByIDWithRelations(id, true);

            if (equipment == null)
                throw new EquipmentNotFoundException(id);

            if (equipment.Durum != "Bekliyor")
                throw new EquipmentAlreadyProcessedException(id, equipment.Durum);

            equipment.Durum = "Onaylandı";
            equipment.OnaylayanId = user.Id;
            _manager.Equipment.UpdateOneEquipment(id, _mapper.Map<EquipmentDtoForUpdate>(equipment), true);
            return Ok(equipment);
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

            var equipment = _manager.Equipment.GetOneEquipmentByIDWithRelations(id, true);

            if (equipment == null)
                throw new EquipmentNotFoundException(id);


            if (equipment.Durum != "Bekliyor")
                throw new EquipmentAlreadyProcessedException(id, equipment.Durum);

            equipment.Durum = "Reddedildi";
            equipment.OnaylayanId = user.Id;
            _manager.Equipment.UpdateOneEquipment(id, _mapper.Map<EquipmentDtoForUpdate>(equipment), true);
            return Ok(equipment);
        }


    }
}