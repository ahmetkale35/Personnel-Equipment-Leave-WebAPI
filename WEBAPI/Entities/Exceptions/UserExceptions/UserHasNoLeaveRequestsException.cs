using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions.UserExceptions
{
    public class UserHasNoLeaveRequestsException : NotFoundException
    {
        public UserHasNoLeaveRequestsException(string userId)
            : base($"User with id: {userId} has no leave requests in the database.")
        {
        }
    }
}
