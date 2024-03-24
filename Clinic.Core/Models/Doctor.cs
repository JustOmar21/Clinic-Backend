using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Models
{
    public enum Status { Active , Inactive , Banned }
    public enum Gender { Male , Female , PreferNotToSay}
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Governance { get; set; }
        public string Address { get; set; }
        public int NationalID { get; set; }
        public string Phone { get; set; }
        public Gender Gender {  get; set; }
        public DateTime DOB { get; set; }
        public int AppointmentPrice { get; set; }
        public Status Status { get; set; }
        public int? SpecialityID { get; set; }
        public int? ScheduleID { get; set; }

        public virtual Speciality? Speciality { get; set; }
        public virtual Schedule? Schedule { get; set; }
        public virtual IEnumerable<Review>? Reviews { get; set; }
        public virtual IEnumerable<Appointement>? Appointements { get; set; }


    }
}
