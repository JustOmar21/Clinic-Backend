using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Clinic.Core.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Gender Gender { get; set; }
        public DateTime DOB { get; set; }
        public string? PicPath { get; set; }
        public Status Status { get; set; }
        [JsonIgnore]
        public virtual IEnumerable<Review>? Reviews { get; set; }
        [JsonIgnore]
        public virtual IEnumerable<Appointement>? Appointements { get; set; }
    }
}
