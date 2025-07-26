namespace Entities.Exceptions.LeaveExceptions
{
    public sealed class LeaveRequestNotFoundException : NotFoundException
    {
        public LeaveRequestNotFoundException(int id) 
            : base($"Leave request with id: {id} does not exist in the database.") { }
    }
}
