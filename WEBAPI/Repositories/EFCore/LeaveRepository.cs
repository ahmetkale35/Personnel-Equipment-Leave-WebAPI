using Entities.DataTransferObject;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
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
        public IQueryable<LeaveRequest> GetAllLeaves(bool trackChanges) => 
            FindAll(trackChanges);
        public IEnumerable<LeaveRequest> GetAllLeavesWithRelations(bool trackChanges)
        {
            return trackChanges
                ? _context.LeaveRequests
                    .Include(l => l.User)
                    .Include(l => l.LeaveType)
                    .ToList()
                : _context.LeaveRequests
                    .AsNoTracking()
                    .Include(l => l.User)
                    .Include(l => l.LeaveType)
                    .ToList();
        }
        public IQueryable<LeaveRequest> GetOneLeaveById(int id, bool trackChanges) => 
            FindByCondition(x => x.id.Equals(id), trackChanges);
        public LeaveRequest GetOneLeaveByIDWithRelations(int id, bool trackChanges)
        {
            return trackChanges
        ? _context.LeaveRequests
            .Include(lr => lr.User)
            .Include(lr => lr.LeaveType)
            .FirstOrDefault(lr => lr.id == id)
        : _context.LeaveRequests
            .AsNoTracking()
            .Include(lr => lr.User)
            .Include(lr => lr.LeaveType)
            .FirstOrDefault(lr => lr.id == id);
        }
        public void UpdateOneLeave(LeaveRequest leave) => Update(leave);

    }
}
