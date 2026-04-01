using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace Repositories.EFCore.Extensions
{
    public static class LeaveRepositoryExtensions
    {
        public static IQueryable<LeaveRequest> Search(this IQueryable<LeaveRequest> query, string searchTerm)
        {
            // If the search term is empty, just return the query as-is
            if (string.IsNullOrWhiteSpace(searchTerm))
                return query;

            var lowerCaseTerm = searchTerm.ToLower();

            // Check if the search term happens to be a valid date
            bool isDateSearch = DateTime.TryParse(searchTerm, out DateTime searchDate);

            return query.Where(l =>
                // String comparisons (EF Core can translate these)
                l.User.UserName.ToLower().Contains(lowerCaseTerm) ||
                l.LeaveType.Ad.ToLower().Contains(lowerCaseTerm) ||

                // Date comparisons (Compare actual DateTimes, not strings)
                (isDateSearch && l.BaslangicTarihi.Date == searchDate.Date) ||
                (isDateSearch && l.BitisTarihi.Date == searchDate.Date)
            );
        }

        public static IQueryable<LeaveRequest> Sort(this IQueryable<LeaveRequest> leaves, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return leaves.OrderBy(b => b.id);

            var orderQuery = OrderQueryBuilder
                .CreateOrderQuery<EquipmentRequests>(orderByQueryString);

            if (orderQuery is null)
                return leaves.OrderBy(b => b.id);

            return leaves.OrderBy(orderQuery);


        }
    }
}
