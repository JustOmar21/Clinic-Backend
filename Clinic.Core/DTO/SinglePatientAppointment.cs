using Clinic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.DTO
{
    public class SinglePatientAppointment
    {
        public Patient Patient { get; set; }
        public Appointement Appointement { get; set; }
    }
}
