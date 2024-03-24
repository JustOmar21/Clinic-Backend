using Clinic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Repos
{
    public interface IPatientRepo
    {
        public int GetPatientCount();
        public List<Patient> GetAllPatient(int pageNumber = 1, int pageSize = 10, string name = "" , string email = "");
        public dynamic GetPatient(int id);
        public dynamic GetAppointements(int DoctorID, int PatientID);
        public dynamic GetAllAppointements(int PatientID, DateTime? date = null);
        public HttpStatusCode AddPatient(Patient patient);
        public HttpStatusCode EditPatient(Patient patient);
        public HttpStatusCode DeletePatient(int id);
        public HttpStatusCode AddAppointment(Appointement app);
    }
}
