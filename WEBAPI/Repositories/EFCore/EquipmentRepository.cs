using Entities.Models;
using Entities.RequestFeatures;
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

        //public IQueryable<EquipmentRequests> GetAllApprovedEquipments(bool trackChanges)
        //{
        //    return trackChanges
        //        ? FindByCondition(e => e.Durum.Equals("Onaylandı", StringComparison.OrdinalIgnoreCase), trackChanges)
        //        : FindByCondition(e => e.Durum.Equals("Onaylandı", StringComparison.OrdinalIgnoreCase), false);
        //}

        public PagedList<EquipmentRequests> GetAllEquipmentsWithRelations(EquipmentParameters equipmentParameter, bool trackChanges)
        {

            var equipment = FindAll(trackChanges)
                .Include(er => er.User)            // Talebi yapan kullanıcı
                .Include(er => er.Onaylayan)       // Onaylayan kullanıcı (nullable olabilir)
                .Include(er => er.EquipmentItem)   // Talep edilen ekipman
                
                .ToList();

            return PagedList<EquipmentRequests>.ToPagedList(equipment, 
                equipmentParameter.PageNumber, 
                equipmentParameter.PageSize);





















            //    trackChanges
            //? _context.EquipmentRequests
            //    .Include(er => er.User)           // Talebi yapan kullanıcı
            //    .Include(er => er.Onaylayan)      // Onaylayan kullanıcı (nullable)
            //    .Include(er => er.EquipmentItem)
            //    .Skip((equipmentParameter.PageNumber - 1) * equipmentParameter.PageSize)
            //    .Take(equipmentParameter.PageSize)// Talep edilen ekipman
            //    .ToList()
            //: _context.EquipmentRequests
            //    .AsNoTracking()
            //    .Include(er => er.User)
            //    .Include(er => er.Onaylayan)
            //    .Include(er => er.EquipmentItem)
            //    .Skip((equipmentParameter.PageNumber - 1) * equipmentParameter.PageSize)
            //    .Take(equipmentParameter.PageSize)
            //    .ToList();


        }

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


        public int stockCount(int id, bool trackChanges)
        {
            var equipment = trackChanges
                ? _context.EquipmentItems.FirstOrDefault(e => e.Id == id)
                : _context.EquipmentItems.AsNoTracking().FirstOrDefault(e => e.Id == id);
            return equipment?.Adet ?? 0;
        }

        public PagedList<EquipmentRequests> GetAllEquipments(EquipmentParameters equipmentParameter,
            bool trackChanges)
        {
            var equipment = FindAll(trackChanges)
                .ToList();

            return PagedList<EquipmentRequests>.ToPagedList(equipment,
                equipmentParameter.PageNumber,
                equipmentParameter.PageSize);


        }
           

        public IQueryable<EquipmentItem> GetAllStocks(bool trackChanges)
        {
            return trackChanges
                ? _context.EquipmentItems
                : _context.EquipmentItems.AsNoTracking();

        }
    }
}
