using AutoMapper;
using Entities.DataTransferObject.EquipmentDTO;
using Entities.Exceptions.EquipmentExceptions;
using Entities.Exceptions.UserExceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class EquipmentManager : IEquipmentService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        public EquipmentManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
        }

        public EquipmentDto CreateOneEquipment(EquipmentDtoInsertion equipmentDtoInsertion)
        {
            var equipmentRequest = _mapper.Map<EquipmentRequests>(equipmentDtoInsertion);
            _manager.Equipment.CreateOneEquipment(equipmentRequest);
            _manager.Save();
            return _mapper.Map<EquipmentDto>(equipmentRequest);
        }

        public void DeleteOneEquipment(int id, bool trackChanges)
        {
            var entity = _manager.Equipment.GetOneEquipmentByIDWithRelations(id, trackChanges);
            if (entity == null)
                throw new EquipmentNotFoundException(id);

            _manager.Equipment.DeleteOneEquipment(entity);
            _manager.Save();
        }

        public IEnumerable<EquipmentDto> GetAllEquipments(bool trackChanges)
        {
            return _mapper.Map<IEnumerable<EquipmentDto>>(trackChanges ? _manager.Equipment.GetAllEquioments(trackChanges) 
                : _manager.Equipment.GetAllEquioments(false));
        }

        public IEnumerable<EquipmentDto> GetAllEquipmentsWithRelations(bool trackChanges)
        {
            return _mapper.Map<IEnumerable<EquipmentDto>>(trackChanges
                ? _manager.Equipment.GetAllEquipmentsWithRelations(trackChanges) 
                : _manager.Equipment.GetAllEquipmentsWithRelations(false));
        }

        public EquipmentDto GetOneEquipmentByID(int id, bool trackChanges)
        {
            return _mapper.Map<EquipmentDto>(
                trackChanges ? _manager.Equipment.GetOneEquipmentById(id, trackChanges) 
                : _manager.Equipment.GetOneEquipmentById(id, false));
        }

        public EquipmentDto GetOneEquipmentByIDWithRelations(int id, bool trackChanges)
        {
            var equipment = _manager.Equipment.GetOneEquipmentByIDWithRelations(id, trackChanges);
            if(equipment == null)
                throw new EquipmentNotFoundException(id);

            return _mapper.Map<EquipmentDto>(equipment);
        }

        public IEnumerable<EquipmentDto> MyEquipments(string id, bool trackChanges)
        {
           List<EquipmentRequests> myRequests = _manager.Equipment.GetAllEquipmentsWithRelations(trackChanges)
                .Where(e => e.UserId == id).ToList();
            if (myRequests.Count == 0)
                throw new UserHasNoEquipmentRequestsException($"No equipment requests found for UserId: {id}");
            return _mapper.Map<IEnumerable<EquipmentDto>>(myRequests);
        }

        public IEnumerable<EquipmentDto> Pending(string id, bool trackChanges)
        {
            List<EquipmentDto> pendingRequests = _manager.Equipment
                .GetAllEquipmentsWithRelations(trackChanges)
                .Where(e => e.Durum == "Bekliyor")
                .Select(e => _mapper.Map<EquipmentDto>(e))
                .ToList();
            if (pendingRequests.Count == 0)
                throw new UserHasNoEquipmentRequestsException(id);
            return pendingRequests;
        }

        public void UpdateOneEquipment(int id, EquipmentDtoForUpdate equipmentDto, bool trackChanges)
        {
            var entity = _manager.Equipment.GetOneEquipmentByIDWithRelations(id, trackChanges);
            if (entity == null)
                throw new EquipmentNotFoundException(id);

            if (equipmentDto == null)
                throw new ArgumentNullException(nameof(equipmentDto), "Leave request cannot be null");

            _mapper.Map(equipmentDto, entity);
            _manager.Equipment.UpdateOneEquipment(entity);
            _manager.Save();
        }
    }
}
