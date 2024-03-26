using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Clinic.Core.Models
{
    public class Paycard
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string ExpirationDate { get; set; }
        public string CCV { get; set; }
        public int PatientID { get; set; }
        [JsonIgnore]
        public virtual Patient? Patient { get; set; }

    }
}
