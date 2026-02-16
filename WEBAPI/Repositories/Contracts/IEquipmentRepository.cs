using Entities.DataTransferObject.EquipmentDTO;
using Entities.Models;
using Entities.RequestFeatures;


namespace Repositories.Contracts
{
    public interface IEquipmentRepository
    {

        IQueryable<EquipmentRequests> GetAllEquipments(EquipmentParameters equipmentParameter, bool trackChanges);
       // IQueryable<EquipmentRequests> GetAllApprovedEquipments(bool trackChanges);
        IQueryable<EquipmentRequests> GetOneEquipmentById(int id, bool trackChanges);

        IQueryable<EquipmentItem> GetAllStocks(bool trackChanges);
        void CreateOneEquipment(EquipmentRequests equipment);
        void UpdateOneEquipment(EquipmentRequests equipment);
        void DeleteOneEquipment(EquipmentRequests equipment);

        int stockCount(int id, bool trackChanges);
        // Gets all leaves with their relations (e.g., User and LeaveType)
        IEnumerable<EquipmentRequests> GetAllEquipmentsWithRelations(EquipmentParameters equipmentParameter,bool trackChanges);


        // Gets a single leave by ID with its relations
        EquipmentRequests GetOneEquipmentByIDWithRelations(int id, bool trackChanges);
    }
}
