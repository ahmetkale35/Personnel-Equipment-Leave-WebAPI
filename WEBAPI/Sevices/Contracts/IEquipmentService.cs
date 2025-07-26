
using System.Threading.Tasks;
using Entities.DataTransferObject.EquipmentDTO;


namespace Services.Contracts
{
    public interface IEquipmentService
    {
        IEnumerable<EquipmentDto> GetAllEquipments(bool trackChanges);
        EquipmentDto GetOneEquipmentByID(int id, bool trackChanges);
        EquipmentDto CreateOneEquipment(EquipmentDtoInsertion equipment);
        void UpdateOneEquipment(int id, EquipmentDtoForUpdate equipmentDto, bool trackChanges);
        void DeleteOneEquipment(int id, bool trackChanges);
        
        IEnumerable<EquipmentDto> GetAllEquipmentsWithRelations(bool trackChanges);
        EquipmentDto GetOneEquipmentByIDWithRelations(int id, bool trackChanges);
        
        IEnumerable<EquipmentDto> MyEquipments(string id, bool trackChanges);

        IEnumerable<EquipmentDto> Pending(string id, bool trackChanges);
    }
}
