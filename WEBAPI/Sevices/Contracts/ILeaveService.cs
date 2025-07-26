
using Entities.DataTransferObject;

namespace Services.Contracts
{
    public interface ILeaveService
    {
        IEnumerable<LeaveRequestDto> GetAllLeaves(bool trackChanges);

        LeaveRequestDto GetOneLeaveByID(int id, bool trackChanges);

        LeaveRequestDto CreateOneLeave(LeaveRequestDtoInsertion leaveRequest);

        void UpdateOneLeave(int id,LeaveRequestDtoForUpdate leaveRequestDto,bool trackChanges);

        void DeleteOneLeave(int id,bool trackChanges);
        IEnumerable<LeaveRequestDto> GetAllLeavesWithRelations(bool trackChanges);

        LeaveRequestDto GetOneLeaveByIDWithRelations(int id, bool trackChanges);

        IEnumerable<LeaveRequestDto> MyRequests(string id, bool trackChanges);

        IEnumerable<LeaveRequestDto> Pending(string id, bool trackChanges);



    }
}
