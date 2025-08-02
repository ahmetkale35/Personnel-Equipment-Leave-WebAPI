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
        public async Task<IEnumerable<LeaveRequest>> GetAllLeavesWithRelationsAsync(bool trackChanges)
        {
            return trackChanges
                ? await _context.LeaveRequests
                    .Include(l => l.User)
                    .Include(l => l.LeaveType)
                    .ToListAsync()
                : await _context.LeaveRequests
                    .AsNoTracking()
                    .Include(l => l.User)
                    .Include(l => l.LeaveType)
                    .ToListAsync();
        }

        public async Task<LeaveRequest> GetOneLeaveByIdAsync(int id, bool trackChanges) => 
            await FindByCondition(x => x.id.Equals(id), trackChanges).SingleOrDefaultAsync();

        public async Task<LeaveRequest> GetOneLeaveByIDWithRelationsAsync(int id, bool trackChanges)
        {
            return trackChanges
        ? await _context.LeaveRequests
            .Include(lr => lr.User)
            .Include(lr => lr.LeaveType)
            .FirstOrDefaultAsync(lr => lr.id == id)
        : await _context.LeaveRequests
            .AsNoTracking()
            .Include(lr => lr.User)
            .Include(lr => lr.LeaveType)
            .FirstOrDefaultAsync(lr => lr.id == id);
        }

        public void UpdateOneLeave(LeaveRequest leave) => Update(leave);

    }
}
