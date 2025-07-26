
namespace Entities.Exceptions.LeaveExceptions
{
    public class LeaveAlreadyProcessedException :NotFoundException
    {
        public LeaveAlreadyProcessedException(int id, string status)
        : base($"Leave with ID {id} has already been processed. Current status: {status}")
        {
        }
    }
}
