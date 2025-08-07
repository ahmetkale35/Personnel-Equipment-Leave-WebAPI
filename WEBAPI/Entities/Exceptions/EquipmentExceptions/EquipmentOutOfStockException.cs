using Entities.Exceptions;
using System.Runtime.Serialization;

namespace Presentation.Controller
{
    [Serializable]
    public class EquipmentOutOfStockException : NotFoundException
    {
       public EquipmentOutOfStockException(int equipmentItemId, int stockCount)
            : base($"Equipment with ID {equipmentItemId} is out of stock. Current stock count: {stockCount}.") {}
        

    }
}