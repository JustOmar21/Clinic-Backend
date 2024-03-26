using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Clinic.Core.Models
{
    public enum AppStatus { Accepted , Rejected , Cancaled };
    public class Appointement
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Order { get; set; }
        public AppStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        [JsonIgnore]
        public virtual Patient? Patient { get; set; }
        [JsonIgnore]
        public virtual Doctor? Doctor { get; set; }
    }
}
