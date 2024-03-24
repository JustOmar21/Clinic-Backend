using Clinic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.DTO
{
    public class SingleDoctorDetails
    {
        public Doctor? Doctor { get; set; }
        public Speciality? Speciality { get; set; }
        public Schedule? Schedule { get; set; }
        public List<Review>? Reviews { get; set; }
    }
}
