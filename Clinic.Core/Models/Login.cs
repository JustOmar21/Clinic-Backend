using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Models
{
    public class Login
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string type { get; set; }
    }
}
