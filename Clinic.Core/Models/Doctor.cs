using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Governance { get; set; }
        public string Address { get; set; }
        public int NationalID { get; set; }
        public string Phone { get; set; }
        public int Gender {  get; set; }
        public DateTime DOB { get; set; }
        public int AppointmentPrice { get; set; }
        public int Status { get; set; }

    }
}
