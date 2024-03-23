using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Notes { get; set; }
        public int Score { get; set; }
        public int DoctorID { get; set; }
        public int PatientID { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }

    }
}
