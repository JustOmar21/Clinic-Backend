using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Clinic.Core.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Notes { get; set; }
        public int Score { get; set; }
        public DateTime? date { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        [JsonIgnore]
        public virtual Doctor? Doctor { get; set; }
        [JsonIgnore]
        public virtual Patient? Patient { get; set; }

    }
}
