using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions.EquipmentExceptions
{
    public class InsufficientEquipmentStockException : NotFoundException
    {
        public InsufficientEquipmentStockException(int id)
        : base($"Requested quantity exceeds available stock for equipment with ID: {id}.")
        {
        }
    }
}
