
namespace Entities.Exceptions.EquipmentExceptions
{
    public class EquipmentAlreadyProcessedException : NotFoundException
    {
        public EquipmentAlreadyProcessedException(int id, string status)
        : base($"Equipment with ID {id} has already been processed. Current status: {status}")
        {
        }
    }
}