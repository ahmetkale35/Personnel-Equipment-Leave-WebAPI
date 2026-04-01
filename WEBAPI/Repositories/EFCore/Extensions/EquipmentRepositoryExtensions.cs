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

        public static IQueryable<EquipmentRequests> Sort(this IQueryable<EquipmentRequests> equipment, string OrderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(OrderByQueryString))
                return equipment.OrderBy(b => b.Id);

            var orderParams = OrderByQueryString.Trim().Split(',');

            var propertyInfos = typeof(EquipmentRequests)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var orderQueryBuilder = new StringBuilder();
            
            foreach ( var param in orderParams)
            {
                if (string.IsNullOrEmpty(param))
                    continue;

                var propertyFromQueryName = param.Split(' ')[0];

                var objectProperty = propertyInfos
                    .FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName,
                    StringComparison.InvariantCultureIgnoreCase));

                if(objectProperty is null)
                    continue;

                var direction = param.EndsWith("desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");

            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (orderQuery is null)
                return equipment.OrderBy(b => b.Id);

            return equipment.OrderBy(orderQuery);


        }



        //public static IQueryable<Entities.Models.EquipmentRequests> Sort(this IQueryable<Entities.Models.EquipmentRequests> equipment, string orderBy)
        //{
        //    if (string.IsNullOrWhiteSpace(orderBy))
        //        return equipment.OrderBy(e => e.Id); // Varsayılan sıralama
        //    return orderBy switch
        //    {
        //        "username" => equipment.OrderBy(e => e.User.UserName),
        //        "username_desc" => equipment.OrderByDescending(e => e.User.UserName),
        //        "approver" => equipment.OrderBy(e => e.Onaylayan != null ? e.Onaylayan.UserName : string.Empty),
        //        "approver_desc" => equipment.OrderByDescending(e => e.Onaylayan != null ? e.Onaylayan.UserName : string.Empty),
        //        "equipment" => equipment.OrderBy(e => e.EquipmentItem.Ad),
        //        "equipment_desc" => equipment.OrderByDescending(e => e.EquipmentItem.Ad),
        //        _ => equipment.OrderBy(e => e.Id) // Geçersiz sıralama parametresi için varsayılan sıralama
        //    };
        //}
    }
}
