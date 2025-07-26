using Entities.Models;


namespace Repositories.Contracts
{
    public interface ILeaveRepository : IRepositoryBase<LeaveRequest>
    {
        IQueryable<LeaveRequest> GetAllLeaves(bool trackChanges);
        IQueryable<LeaveRequest> GetOneLeaveById(int id, bool trackChanges);

        void CreateOneLeave(LeaveRequest leave);
        void UpdateOneLeave(LeaveRequest leave);
        void DeleteOneLeave(LeaveRequest leave);


        // Gets all leaves with their relations (e.g., User and LeaveType)
        IEnumerable<LeaveRequest> GetAllLeavesWithRelations(bool trackChanges);


        // Gets a single leave by ID with its relations
        LeaveRequest GetOneLeaveByIDWithRelations(int id, bool trackChanges);



    }
}
