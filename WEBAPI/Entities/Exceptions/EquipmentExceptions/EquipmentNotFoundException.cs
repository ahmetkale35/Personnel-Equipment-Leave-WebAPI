
namespace Entities.Exceptions.EquipmentExceptions
{
    public  class EquipmentNotFoundException : NotFoundException
    {
        public EquipmentNotFoundException(int id)
            : base($"Equipment request with id: {id} does not exist in the database.") { }
    }
}
