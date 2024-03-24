using Clinic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.DTO
{
    public class PatientAppointments
    {
        public Patient Patient { get; set; }
        public List<Appointement> Appointements { get; set; }
    }
}
