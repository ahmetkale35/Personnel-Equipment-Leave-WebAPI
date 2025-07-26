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
        
        public LeaveRequestDto CreateOneLeave(LeaveRequestDtoInsertion leaveRequestDtoIns)
        {
           var entity = _mapper.Map<LeaveRequest>(leaveRequestDtoIns);
            _manager.Leave.CreateOneLeave(entity);   
            _manager.Save();
            return _mapper.Map<LeaveRequestDto>(entity);
        }

        public void DeleteOneLeave(int id, bool trackChanges)
        {
            var entity = _manager.Leave.GetOneLeaveByIDWithRelations(id, trackChanges);
            if (entity == null)
                throw new LeaveRequestNotFoundException(id);
            
            _manager.Leave.DeleteOneLeave(entity);
            _manager.Save();

        }

        public IEnumerable<LeaveRequestDto> GetAllLeaves(bool trackChanges)
        {
            return _mapper.Map<IEnumerable<LeaveRequestDto>>(
                trackChanges ? _manager.Leave.GetAllLeaves(trackChanges) : _manager.Leave.GetAllLeaves(false));
            
        }

        public IEnumerable<LeaveRequestDto> GetAllLeavesWithRelations(bool trackChanges)
        {
            // AutoMapper ile IEnumerable LeaveRequestDto'ya dönüştürme
            return _mapper.Map<IEnumerable<LeaveRequestDto>>(trackChanges
                ? _manager.Leave.GetAllLeavesWithRelations(trackChanges)
                : _manager.Leave.GetAllLeavesWithRelations(false));
        }

        public LeaveRequestDto GetOneLeaveByID(int id, bool trackChanges)
        {
            return _mapper.Map<LeaveRequestDto>(
                _manager.Leave.GetOneLeaveById(id, trackChanges).FirstOrDefault());

        }

        public LeaveRequestDto GetOneLeaveByIDWithRelations(int id, bool trackChanges)
        {
            var leave =  _manager.Leave.GetOneLeaveByIDWithRelations(id, trackChanges);
            if (leave is null)
                throw new LeaveRequestNotFoundException(id);

            return _mapper.Map<LeaveRequestDto>(leave);
        }

        public IEnumerable<LeaveRequestDto> MyRequests(string id, bool trackChanges)
        {
           List<LeaveRequestDto> myRequests = _manager.Leave.GetAllLeavesWithRelations(trackChanges)
                .Where(l => l.UserId == id)
                .Select(l => _mapper.Map<LeaveRequestDto>(l))
                .ToList();
            if (myRequests.Count == 0)
                throw new UserHasNoLeaveRequestsException(id);
            return myRequests;


        }

        public IEnumerable<LeaveRequestDto> Pending(string id, bool trackChanges)
        {
            List<LeaveRequestDto> pendingRequests = _manager.Leave
                .GetAllLeavesWithRelations(trackChanges)
                .Where(l => l.Durum == "Bekliyor")
                .Select(l => _mapper.Map<LeaveRequestDto>(l))
                .ToList();
            if (pendingRequests.Count == 0)
                throw new UserHasNoLeaveRequestsException(id);
            return pendingRequests;
        }

        public void UpdateOneLeave(int id, LeaveRequestDtoForUpdate leaveRequestDto, bool trackChanges)
        {                                  //?
            var entity = _manager.Leave.GetOneLeaveById(id, trackChanges).FirstOrDefault();
            if (entity == null)
                throw new LeaveRequestNotFoundException(id);


            if (leaveRequestDto == null)
                throw new ArgumentNullException(nameof(leaveRequestDto), "Leave request cannot be null");


            /// Alan güncellemeleri Manuel
            //entity.BaslangicTarihi = leaveRequest.BaslangicTarihi;
            //entity.BitisTarihi = leaveRequest.BitisTarihi;
            //entity.Aciklama = leaveRequest.Aciklama;
            //entity.Durum = leaveRequest.Durum;
            //entity.LeaveTypeId = leaveRequest.LeaveTypeId;
            //entity.UserId = leaveRequest.UserId;



            // AutoMapper ile güncelleme
            _mapper.Map(leaveRequestDto, entity);
            _manager.Leave.UpdateOneLeave(entity);
            _manager.Save();
        }

    }
}
