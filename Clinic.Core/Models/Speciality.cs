using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Models
{
    public class Speciality
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<Doctor>? Doctors { get; set; }
    }
}
