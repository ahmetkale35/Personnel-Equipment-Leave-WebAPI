using AutoMapper;
using Entities.DataTransferObject;
using Entities.Exceptions.LeaveExceptions;
using Entities.Exceptions.UserExceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class LeaveManager : ILeaveService
    {

        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        

        public LeaveManager(IRepositoryManager manager,ILoggerService logger, IMapper mapper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<LeaveRequestDto> CreateOneLeaveAsync(LeaveRequestDtoInsertion leaveRequestDtoIns)
        {
           var entity = _mapper.Map<LeaveRequest>(leaveRequestDtoIns);
            _manager.Leave.CreateOneLeave(entity);   
            await _manager.SaveAsync();
            return _mapper.Map<LeaveRequestDto>(entity);
        }

        public async Task DeleteOneLeaveAsync(int id, bool trackChanges)
        {
            var entity = await _manager.Leave.GetOneLeaveByIDWithRelationsAsync(id, trackChanges);
            if (entity == null)
                throw new LeaveRequestNotFoundException(id);

            _manager.Leave.DeleteOneLeave(entity);
            await _manager.SaveAsync();
        }


        public async Task<IEnumerable<LeaveRequestDto>> GetAllLeavesAsync(bool trackChanges)
        {
            return _mapper.Map<IEnumerable<LeaveRequestDto>>(
                trackChanges ? await _manager.Leave.GetAllLeavesWithRelationsAsync(trackChanges) : await _manager.Leave.GetAllLeavesWithRelationsAsync(false));
            
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetAllLeavesWithRelationsAsync(bool trackChanges)
        {
            // AutoMapper ile IEnumerable LeaveRequestDto'ya dönüştürme
            return _mapper.Map<IEnumerable<LeaveRequestDto>>(trackChanges
                ? await _manager.Leave.GetAllLeavesWithRelationsAsync(trackChanges)
                : await _manager.Leave.GetAllLeavesWithRelationsAsync(false));
        }

        public async Task<LeaveRequestDto?> GetOneLeaveByIDAsync(int id, bool trackChanges)
        {
            var leave = await _manager.Leave.GetOneLeaveByIdAsync(id, trackChanges);

            if (leave == null)
                throw new LeaveRequestNotFoundException(id);

            return _mapper.Map<LeaveRequestDto>(leave);
        }


        public async Task<LeaveRequestDto> GetOneLeaveByIDWithRelationsAsync(int id, bool trackChanges)
        {
            var leave =  await _manager.Leave.GetOneLeaveByIDWithRelationsAsync(id, trackChanges);
            if (leave is null)
                throw new LeaveRequestNotFoundException(id);

            return _mapper.Map<LeaveRequestDto>(leave);
        }

        public async Task<IEnumerable<LeaveRequestDto>> MyRequestsAsync(string id, bool trackChanges)
        {
            var allLeaves = await _manager.Leave.GetAllLeavesWithRelationsAsync(trackChanges);

            List<LeaveRequestDto> myRequests = allLeaves
                .Where(l => l.UserId == id)
                .Select(l => _mapper.Map<LeaveRequestDto>(l))
                .ToList();

            if (myRequests.Count == 0)
                throw new UserHasNoLeaveRequestsException(id);

            return myRequests;
        }


        public async Task<IEnumerable<LeaveRequestDto>> PendingAsync(string id, bool trackChanges)
        {
            var allLeaves = await _manager.Leave.GetAllLeavesWithRelationsAsync(trackChanges);

            List<LeaveRequestDto> pendingRequests = allLeaves
                .Where(l => l.Durum == "Bekliyor")
                .Select(l => _mapper.Map<LeaveRequestDto>(l))
                .ToList();

            if (pendingRequests.Count == 0)
                throw new UserHasNoLeaveRequestsException(id);

            return pendingRequests;
        }


        public async Task UpdateOneLeaveAsync(int id, LeaveRequestDtoForUpdate leaveRequestDto, bool trackChanges)
        {
            var entity = await _manager.Leave.GetOneLeaveByIDWithRelationsAsync(id, trackChanges);
            if (entity == null)
                throw new LeaveRequestNotFoundException(id);

            if (leaveRequestDto == null)
                throw new ArgumentNullException(nameof(leaveRequestDto), "Leave request cannot be null");

            // AutoMapper ile güncelleme
            _mapper.Map(leaveRequestDto, entity);
            _manager.Leave.UpdateOneLeave(entity);

            await _manager.SaveAsync();
        }


    }
}
