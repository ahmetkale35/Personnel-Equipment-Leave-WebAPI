using Entities.DataTransferObject;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public class LeaveRepository : RepositoryBase<LeaveRequest>, ILeaveRepository
    {
        public LeaveRepository(RepositoryContext context) : base(context) { }
        public void CreateOneLeave(LeaveRequest leave) => Create(leave);
        public void DeleteOneLeave(LeaveRequest leave) => Delete(leave);
        public IQueryable<LeaveRequest> GetAllLeaves(LeaveParameter leaveParameter, bool trackChanges) => 
            FindAll(trackChanges);
        public IEnumerable<LeaveRequest> GetAllLeavesWithRelations(LeaveParameter leaveParameter, bool trackChanges)
        {
            // 1. Start with the base query and includes
            var query = _context.LeaveRequests
                .Include(l => l.User)
                .Include(l => l.LeaveType)
                .AsQueryable();

            // 2. Conditionally apply AsNoTracking
            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            // 3. Apply search and pagination, then execute with ToList()
            return query
                .Search(leaveParameter.SearchTerm)
                .Skip((leaveParameter.PageNumber - 1) * leaveParameter.PageSize)
                .Take(leaveParameter.PageSize) // Talep edilen ekipman (Requested equipment)
                .ToList();
        }
        public IQueryable<LeaveRequest> GetOneLeaveById(int id, bool trackChanges) => 
            FindByCondition(x => x.id.Equals(id), trackChanges);
        public LeaveRequest? GetOneLeaveByIDWithRelations(int id, bool trackChanges)
        {
            var query = _context.LeaveRequests
                .Include(lr => lr.User)
                .Include(lr => lr.LeaveType)
                .AsQueryable();

            if (!trackChanges)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefault(lr => lr.id == id);
        }
        public void UpdateOneLeave(LeaveRequest leave) => Update(leave);

    }
}
