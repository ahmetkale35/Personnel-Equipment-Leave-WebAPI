using AutoMapper;
using Entities.DataTransferObject;
using Entities.Exceptions.LeaveExceptions;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Security.Claims;


namespace Presentation.Controller
{

    [ApiController]
    [Route("api/[controller]")]
    public class LeavesController : ControllerBase
    {

        private readonly IServiceManager _manager;
        public readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public LeavesController(IServiceManager manager, UserManager<User> userManager,IMapper mapper)
        {
            _manager = manager;
            _userManager = userManager;
            _mapper = mapper;
        }

       
        [HttpGet]
        [Route("GetAllLeaves")]
        public async Task<ActionResult> GetAllLeavesAsync()
        {
            var leaves = await _manager.Leave.GetAllLeavesWithRelationsAsync(false);
            return Ok(leaves);
        }

        [Authorize]
        [HttpGet]
        [Route("GetOneLeave/{id:int}")]
        public async Task<IActionResult> GetOneLeaveAsync([FromRoute(Name = "id")] int id)
        {
            var leave = await _manager.Leave.GetOneLeaveByIDWithRelationsAsync(id, false);

            if (leave is null)
                throw new LeaveRequestNotFoundException(id);

            return Ok(leave);
        }

        [Authorize]
        [HttpPost]
        [Route("CreateOneLeave")]
        public async Task<IActionResult> CreateOneBookAsync([FromBody] LeaveRequestDtoInsertion leaveRequestDtoInsert)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            var user = _userManager.FindByNameAsync(username).Result;
            if (user == null)
                return Unauthorized();

            if (leaveRequestDtoInsert is null)
                return BadRequest(); // 400  

            leaveRequestDtoInsert.UserId = user.Id; // Set the Id of the created leave  

            var createdLeave = await _manager.Leave.CreateOneLeaveAsync(leaveRequestDtoInsert);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState); // 422  

            return StatusCode(201, createdLeave); // CreatedAtRoute()  
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateOneLeave/{id:int}")]
        public async Task<IActionResult> UpdateOneLeaveAsync([FromRoute(Name = "id")] int id,
            [FromBody] LeaveRequestDtoForUpdate leaveRequestDto)
        {
            if (leaveRequestDto is null)
                return BadRequest(); // 400

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState); // 422

            await _manager.Leave.UpdateOneLeaveAsync(id, leaveRequestDto, false);
            return NoContent(); // 204 
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteOneLeave/{id:int}")]
        public async Task<IActionResult> DeleteOneBookAsync([FromRoute(Name = "id")] int id)
        {
            await _manager.Leave
           .DeleteOneLeaveAsync(id, true);
            return NoContent();
        }


        [Authorize]
        [HttpGet]
        [Route("MyRequests")]
        public async Task<IActionResult> MyRequestsAsync(bool trackChanges)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return Unauthorized();
            var user = _userManager.FindByNameAsync(username).Result;
            if (user == null)
                return Unauthorized();
            var leaves = await _manager.Leave.MyRequestsAsync(user.Id, trackChanges);
            return Ok(leaves);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Pending")]
        public async Task<IActionResult> PendingAsync(bool trackChanges)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
                return Unauthorized();
            var user = _userManager.FindByNameAsync(username).Result;
            if (user == null)
                return Unauthorized();
            var leaves = await _manager.Leave.PendingAsync(user.Id, trackChanges);
            return Ok(leaves);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id:int}/approve")]
        public async Task<IActionResult> ApproveAsync([FromRoute(Name = "id")] int id)
        {
            var leave = await _manager.Leave.GetOneLeaveByIDWithRelationsAsync(id, true);

            if (leave == null)
                throw new LeaveNotFoundException(id);

            if (leave.Durum != "Bekliyor")
                throw new LeaveAlreadyProcessedException(id, leave.Durum);

            leave.Durum = "Onaylandı";
            _manager.Leave.UpdateOneLeaveAsync(id, _mapper.Map<LeaveRequestDtoForUpdate>(leave), true);
            return Ok(leave);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id:int}/reject")]
        public async Task<IActionResult> RejectAsync([FromRoute(Name = "id")] int id)
        {
            var leave = await _manager.Leave.GetOneLeaveByIDWithRelationsAsync(id, true);

            if (leave == null)
                throw new LeaveNotFoundException(id);


            if (leave.Durum != "Bekliyor")
                throw new LeaveAlreadyProcessedException(id, leave.Durum);

            leave.Durum = "Reddedildi";
            _manager.Leave.UpdateOneLeaveAsync(id, _mapper.Map<LeaveRequestDtoForUpdate>(leave), true);
            return Ok(leave);
        }


        //[HttpPatch]
        //[Route("PartiallyUpdateOneLeave/{id:int}")]
        //public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id,
        //    [FromBody] JsonPatchDocument<LeaveRequestDtoForUpdate> leavepatch)
        //{ 
        //        var entity = _manager
        //            .Leave
        //            .GetOneLeaveByID(id, true);
        //        if (entity is null)
        //            return NotFound(); // 404

        //    leavepatch.ApplyTo(new LeaveRequestDtoForUpdate(
        //            entity.id,
        //            entity.UserId,
        //            entity.LeaveTypeId,
        //            entity.BaslangicTarihi,
        //            entity.BitisTarihi,
        //            entity.Aciklama,
        //            entity.Durum));
        //        _manager.Leave.UpdateOneLeave(id, new LeaveRequestDtoForUpdate(
        //            entity.id,
        //            entity.UserId,
        //            entity.LeaveTypeId,
        //            entity.BaslangicTarihi,
        //            entity.BitisTarihi,
        //            entity.Aciklama,
        //            entity.Durum), false);

        //        return NoContent(); // 204

        //}

        //[Authorize(Roles = "Employee,It,Admin")]
        //[HttpGet("MyRequests")]
        //public IActionResult MyRequests(bool trackChanges)
        //{
        //    var leaves = _manager.Leave.MyRequests(trackChanges);
        //    return Ok(leaves); // boşsa bile [] döner
        //}

    }
}

