using AutoMapper;
using Entities.DataTransferObject;
using Entities.DataTransferObject.EquipmentDTO;
using Entities.Exceptions.EquipmentExceptions;
using Entities.Exceptions.UserExceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using SQLitePCL;
using System.Security.Claims;

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

        public bool IsEquipmentExists(int equipmentId, bool trackChanges)
        {
            var stockCount = _manager.Equipment.stockCount(equipmentId, trackChanges);
            if (stockCount <= 0)
            {
                _logger.LogWarning($"Equipment with ID {equipmentId} does not exist or is out of stock.");
                return false;
            }
            else
            {
                _logger.LogInfo($"Equipment with ID {equipmentId} exists and is in stock.");
                return true;
            }

        }

        // Stok sayısını döndüren metot
        public int StockCount(int id, bool trackChanges)
        {
            return _manager.Equipment.stockCount(id, trackChanges);
        }

        // Zimmetleme işlemi için gerekli metot
        public void AssignEquipmentToUser(int equipmentId, string userId, bool trackChanges)
        {


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

        public IEnumerable<EquipmentDto> GetAllApprovedEquipments(bool trackChanges)
        {
            List<EquipmentRequests> approvedEquipments = _manager.Equipment
                .GetAllEquipmentsWithRelations(trackChanges)
                .Where(e => e.Durum.Equals("Onaylandı", StringComparison.OrdinalIgnoreCase))
                .ToList();
            if (approvedEquipments.Count == 0)
                throw new UserHasNoEquipmentRequestsException("No approved equipment requests found.");
            return _mapper.Map<IEnumerable<EquipmentDto>>(approvedEquipments);
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
            if (equipment == null)
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

        public EquipmentDto DecreaseCount(int id, bool trackChanges)
        {
            var entity = _manager.Equipment.GetOneEquipmentByIDWithRelations(id, trackChanges);
            if (entity == null)
                throw new EquipmentNotFoundException(id);

            entity.EquipmentItem.Adet = entity.EquipmentItem.Adet - entity.Adet;
            entity.EquipmentItem.Atanan++;
            return _mapper.Map<EquipmentDto>(entity);
        }

        public void CheckEquipmentStock(int id, bool trackChanges)
        {
            var equipment = _manager.Equipment.GetOneEquipmentByIDWithRelations(id, trackChanges);
            var stockCount = _manager.Equipment.stockCount(equipment.EquipmentItemId, trackChanges);
            if (stockCount < equipment.Adet)
            {
                _logger.LogWarning($"Insufficient stock for equipment with ID {id}. Requested: {equipment.Adet}, Available: {stockCount}");
                throw new InsufficientEquipmentStockException(id);
            }
            else
            {
                _logger.LogInfo($"Sufficient stock for equipment with ID {id}. Requested: {equipment.Adet}, Available: {stockCount}");
            }


        }

        public IEnumerable<EquipmentDto> GetAllEquipments(bool trackChanges)
        {
            return _mapper.Map<IEnumerable<EquipmentDto>>(
                 trackChanges ? _manager.Equipment.GetAllEquipments(trackChanges) : _manager.Equipment.GetAllEquipments(false));
        }

        public List<EquipmentItem> GetAllStocks(bool trackChanges)
        {
            return _manager.Equipment
                .GetAllStocks(trackChanges)
                .Select(e => _mapper.Map<EquipmentItem>(e))
                .ToList();
        }
    }
}
