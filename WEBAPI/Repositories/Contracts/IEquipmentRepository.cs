using Entities.Models;


namespace Repositories.Contracts
{
    public interface IEquipmentRepository
    {
        IQueryable<EquipmentRequests> GetAllEquioments(bool trackChanges);
        IQueryable<EquipmentRequests> GetOneEquipmentById(int id, bool trackChanges);

        void CreateOneEquipment(EquipmentRequests equipment);
        void UpdateOneEquipment(EquipmentRequests equipment);
        void DeleteOneEquipment(EquipmentRequests equipment);


        // Gets all leaves with their relations (e.g., User and LeaveType)
        IEnumerable<EquipmentRequests> GetAllEquipmentsWithRelations(bool trackChanges);


        // Gets a single leave by ID with its relations
        EquipmentRequests GetOneEquipmentByIDWithRelations(int id, bool trackChanges);
    }
}
