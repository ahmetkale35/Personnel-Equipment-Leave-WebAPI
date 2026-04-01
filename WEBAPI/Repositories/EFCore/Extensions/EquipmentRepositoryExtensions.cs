using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Extensions
{
    public static class EquipmentRepositoryExtensions
    {
        public static IQueryable<Entities.Models.EquipmentRequests> Search(this IQueryable<Entities.Models.EquipmentRequests> equipment, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return equipment;
            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();
            return equipment.Where(e =>
                e.User.UserName.ToLower().Contains(lowerCaseSearchTerm) ||       // Talebi yapan kullanıcı adı
                (e.Onaylayan != null && e.Onaylayan.UserName.ToLower().Contains(lowerCaseSearchTerm)) || // Onaylayan kullanıcı adı
                e.EquipmentItem.Ad.ToLower().Contains(lowerCaseSearchTerm)      // Talep edilen ekipman adı
            );
        }
    }
}
