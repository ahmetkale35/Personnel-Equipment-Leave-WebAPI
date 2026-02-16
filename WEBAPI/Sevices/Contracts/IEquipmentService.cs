
using System.Threading.Tasks;
using Entities.DataTransferObject.EquipmentDTO;
using Entities.Models;
using Entities.RequestFeatures;


namespace Services.Contracts
{
    public interface IEquipmentService
    {
        IEnumerable<EquipmentDto> GetAllEquipments(EquipmentParameters equipmentparameter, bool trackChanges);

        IEnumerable<EquipmentDto> GetAllApprovedEquipments(EquipmentParameters equipmentParameter,bool trackChanges);

        List<EquipmentItem> GetAllStocks(bool trackChanges);
        EquipmentDto GetOneEquipmentByID(int id, bool trackChanges);
        EquipmentDto CreateOneEquipment(EquipmentDtoInsertion equipment);
        void UpdateOneEquipment(int id, EquipmentDtoForUpdate equipmentDto, bool trackChanges);
        void DeleteOneEquipment(int id, bool trackChanges);

        int StockCount(int id, bool trackChanges);

        EquipmentDto DecreaseCount(int id, bool trackChanges);
        bool IsEquipmentExists(int id, bool trackChanges);

        void CheckEquipmentStock(int id, bool trackChanges);
        void AssignEquipmentToUser(int equipmentId, string userId, bool trackChanges);
        IEnumerable<EquipmentDto> GetAllEquipmentsWithRelations(EquipmentParameters equipmentParameter,bool trackChanges);
        EquipmentDto GetOneEquipmentByIDWithRelations(int id, bool trackChanges);
        
        IEnumerable<EquipmentDto> MyEquipments(string id, bool trackChanges);

        IEnumerable<EquipmentDto> Pending(string id, bool trackChanges);
        
    }
}
