using Clinic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.DTO
{
    public class AddDoctor
    {
        public Doctor doctor { get; set; }
        public string password { get; set; }
    }
}
