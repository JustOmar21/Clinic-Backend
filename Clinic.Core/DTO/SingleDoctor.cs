using Clinic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.DTO
{
    public class SingleDoctor
    {
        public Doctor? Doctor { get; set; }
        public Speciality? Speciality { get; set; }
    }
}
