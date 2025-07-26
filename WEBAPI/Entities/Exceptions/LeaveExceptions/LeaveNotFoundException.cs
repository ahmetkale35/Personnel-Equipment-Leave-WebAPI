

namespace Entities.Exceptions.LeaveExceptions
{
    public class LeaveNotFoundException : NotFoundException
    {
        public LeaveNotFoundException(int id)
            : base($"Leave request with id: {id} does not exist in the database.") { }
    }
}
