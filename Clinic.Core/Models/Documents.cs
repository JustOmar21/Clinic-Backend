using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Clinic.Core.Models
{
    public enum DocumentType { Certificate , NationalID};
    public class Documents
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public DocumentType Type {  get; set; } 
        public int DoctorID { get; set; }
        [JsonIgnore]
        public Doctor Doctor { get; set; }
    }
}
