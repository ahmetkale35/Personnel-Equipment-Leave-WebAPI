
using Entities.DataTransferObject;

namespace Services.Contracts
{
    public interface ILeaveService
    {
        Task<IEnumerable<LeaveRequestDto>> GetAllLeavesAsync(bool trackChanges);

        Task<LeaveRequestDto> GetOneLeaveByIDAsync(int id, bool trackChanges);

        Task<LeaveRequestDto> CreateOneLeaveAsync(LeaveRequestDtoInsertion leaveRequest);

        Task UpdateOneLeaveAsync(int id,LeaveRequestDtoForUpdate leaveRequestDto,bool trackChanges);

        Task DeleteOneLeaveAsync(int id,bool trackChanges);
        Task<IEnumerable<LeaveRequestDto>> GetAllLeavesWithRelationsAsync(bool trackChanges);

        Task<LeaveRequestDto> GetOneLeaveByIDWithRelationsAsync(int id, bool trackChanges);

        Task<IEnumerable<LeaveRequestDto>> MyRequestsAsync(string id, bool trackChanges);

        Task<IEnumerable<LeaveRequestDto>> PendingAsync(string id, bool trackChanges);



    }
}
