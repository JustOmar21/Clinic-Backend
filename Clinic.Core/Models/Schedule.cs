using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Clinic.Core.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string Saturday { get; set; }
        public string Sunday { get; set; }
        public string Monday { get; set; }
        public string Tuesday { get; set;}
        public string Wednesday { get; set;}
        public string Thursday { get; set;}
        public string Friday { get; set;}
        public int DoctorID { get; set; }
        [JsonIgnore]
        public virtual Doctor? Doctor { get; set; }
    }
}
