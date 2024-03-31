using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.DTO
{
    public class ChangePassword
    {
        public int userID { get; set; }
        public string userRole { get; set; }
        public string password { get; set; }
        public string newPassword { get; set; }
    }
}
