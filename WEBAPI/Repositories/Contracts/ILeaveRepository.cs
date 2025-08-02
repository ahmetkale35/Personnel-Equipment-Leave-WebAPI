using Entities.Models;


namespace Repositories.Contracts
{
    public interface ILeaveRepository : IRepositoryBase<LeaveRequest>
    {
        IQueryable<LeaveRequest> GetAllLeaves(bool trackChanges);
        Task<LeaveRequest> GetOneLeaveByIdAsync(int id, bool trackChanges);

        void CreateOneLeave(LeaveRequest leave);
        void UpdateOneLeave(LeaveRequest leave);
        void DeleteOneLeave(LeaveRequest leave);


        // Gets all leaves with their relations (e.g., User and LeaveType)
        Task<IEnumerable<LeaveRequest>> GetAllLeavesWithRelationsAsync(bool trackChanges);


        // Gets a single leave by ID with its relations
        Task<LeaveRequest?> GetOneLeaveByIDWithRelationsAsync(int id, bool trackChanges);



    }
}
