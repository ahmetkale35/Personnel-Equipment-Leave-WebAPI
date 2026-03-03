using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Extensions
{
    public static class LeaveRepositoryExtensions
    {
        public static IQueryable<Entities.Models.LeaveRequest> Search(this IQueryable<Entities.Models.LeaveRequest> leaves, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return leaves;
            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();
            return leaves.Where(l =>
                l.User.UserName.ToLower().Contains(lowerCaseSearchTerm) ||
                l.LeaveType.Ad.ToLower().Contains(lowerCaseSearchTerm) ||
                l.BaslangicTarihi.ToString("yyyy-MM-dd").Contains(lowerCaseSearchTerm) ||
                l.BitisTarihi.ToString("yyyy-MM-dd").Contains(lowerCaseSearchTerm));
        }
    }
}
