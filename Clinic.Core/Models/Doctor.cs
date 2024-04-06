using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Clinic.Core.Models
{
    public enum Status { Active , Inactive , Banned , Rejected }
    public enum Gender { Male , Female , PreferNotToSay}
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Governance { get; set; }
        public string Address { get; set; }
        public string NationalID { get; set; }
        public string Phone { get; set; }
        public string? Description { get; set; }
        public Gender Gender {  get; set; }
        public DateTime DOB { get; set; }
        public int AppointmentPrice { get; set; }
        public Status Status { get; set; }
        public string? PicPath { get; set; } = "../../Frontend/ITIAngularproject/src/assets/profilepic/defaultDoc.png";
        public int? SpecialityID { get; set; }
        [JsonIgnore]
        public virtual Speciality? Speciality { get; set; }
        [JsonIgnore]
        public virtual IEnumerable<Review>? Reviews { get; set; }
        [JsonIgnore]
        public virtual IEnumerable<Appointement>? Appointements { get; set; }
        [JsonIgnore]
        public virtual IEnumerable<Documents>? Documents { get; set; }


    }
}
