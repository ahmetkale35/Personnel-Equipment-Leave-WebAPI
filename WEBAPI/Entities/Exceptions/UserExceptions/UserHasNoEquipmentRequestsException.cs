using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions.UserExceptions
{
    public class UserHasNoEquipmentRequestsException : NotFoundException
    {
        public UserHasNoEquipmentRequestsException(string userId)
            : base($"User with id: {userId} has no equipment requests in the database.")
        {
        }
    }
}
