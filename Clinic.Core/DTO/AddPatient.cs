using Clinic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.DTO
{
    public class AddPatient
    {
        public Patient patient { get; set; }
        public string password { get; set; }
    }
}
