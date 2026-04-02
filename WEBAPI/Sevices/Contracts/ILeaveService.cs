
using Entities.DataTransferObject;
using Entities.RequestFeatures;
using System.Dynamic;

namespace Services.Contracts
{
    public interface ILeaveService
    {
        IEnumerable<LeaveRequestDto> GetAllLeaves(LeaveParameter leaveParameter, bool trackChanges);

        LeaveRequestDto GetOneLeaveByID(int id, bool trackChanges);

        LeaveRequestDto CreateOneLeave(LeaveRequestDtoInsertion leaveRequest);

        void UpdateOneLeave(int id,LeaveRequestDtoForUpdate leaveRequestDto,bool trackChanges);

        void DeleteOneLeave(int id,bool trackChanges);
        (IEnumerable<ExpandoObject> leaveDtos, MetaData metaData) GetAllLeavesWithRelations(LeaveParameter leaveParameter, bool trackChanges);

        LeaveRequestDto GetOneLeaveByIDWithRelations(int id, bool trackChanges);

        IEnumerable<LeaveRequestDto> MyRequests(string id, bool trackChanges);

        IEnumerable<LeaveRequestDto> Pending(string id, bool trackChanges);



    }
}
