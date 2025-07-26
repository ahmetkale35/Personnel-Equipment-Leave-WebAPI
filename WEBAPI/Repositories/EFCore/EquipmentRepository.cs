using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;


namespace Repositories.EFCore
{
    public class EquipmentRepository : RepositoryBase<EquipmentRequests>, IEquipmentRepository
    {
        public EquipmentRepository(RepositoryContext context) : base(context)
        {

        }

        public void CreateOneEquipment(EquipmentRequests equipment) => Create(equipment);

        public void DeleteOneEquiipment(EquipmentRequests equipment)
        {
            throw new NotImplementedException();
        }

        public void DeleteOneEquipment(EquipmentRequests equipment) => Delete(equipment);

        public IQueryable<EquipmentRequests> GetAllEquioments(bool trackChanges) =>
            FindAll(trackChanges);


        public IEnumerable<EquipmentRequests> GetAllEquipmentsWithRelations(bool trackChanges) =>
    trackChanges
        ? _context.EquipmentRequests
            .Include(er => er.User)           // Talebi yapan kullanıcı
            .Include(er => er.Onaylayan)      // Onaylayan kullanıcı (nullable)
            .Include(er => er.EquipmentItem)  // Talep edilen ekipman
            .ToList()
        : _context.EquipmentRequests
            .AsNoTracking()
            .Include(er => er.User)
            .Include(er => er.Onaylayan)
            .Include(er => er.EquipmentItem)
            .ToList();


        public IQueryable<EquipmentRequests> GetOneEquipmentById(int id, bool trackChanges) =>
             FindByCondition(x => x.Id.Equals(id), trackChanges);


        public EquipmentRequests GetOneEquipmentByIDWithRelations(int id, bool trackChanges)
        {
            return trackChanges
                ? _context.EquipmentRequests
                    .Include(er => er.User)            // Talebi yapan kullanıcı
                    .Include(er => er.Onaylayan)       // Onaylayan kullanıcı (nullable olabilir)
                    .Include(er => er.EquipmentItem)   // Talep edilen ekipman
                    .FirstOrDefault(er => er.Id == id)
                : _context.EquipmentRequests
                    .AsNoTracking()
                    .Include(er => er.User)
                    .Include(er => er.Onaylayan)
                    .Include(er => er.EquipmentItem)
                    .FirstOrDefault(er => er.Id == id);
        }

        public void UpdateOneEquipment(EquipmentRequests equipment) => Update(equipment);

    }
}
