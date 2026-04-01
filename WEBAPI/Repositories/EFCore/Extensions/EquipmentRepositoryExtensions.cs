using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Repositories.EFCore.Extensions
{
    public static class EquipmentRepositoryExtensions
    {
        public static IQueryable<EquipmentRequests> Search(this IQueryable<Entities.Models.EquipmentRequests> equipment, string searchTerm)
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

        public static IQueryable<EquipmentRequests> Sort(this IQueryable<EquipmentRequests> equipment, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return equipment.OrderBy(b => b.Id);

            var orderQuery = OrderQueryBuilder
                .CreateOrderQuery<EquipmentRequests>(orderByQueryString);

            if (orderQuery is null)
                return equipment.OrderBy(b => b.Id);

            return equipment.OrderBy(orderQuery);


        }

    }
}
