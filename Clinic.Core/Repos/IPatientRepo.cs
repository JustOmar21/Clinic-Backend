using Clinic.Core.DTO;
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
        public int GetPatientCount(string name = "", string email = "");
        public List<Patient> GetAllPatient(int pageNumber = 1, int pageSize = 10, string name = "" , string email = "");
        public List<Patient> GetAllPatientNoPage();
        public Patient? GetPatient(int id);
        public SingleDoctorAppointment? GetAppointements(int DoctorID, int PatientID , int pageNumber = 1, int pageSize = 10);
        public int GetAppointementsCount(int DoctorID, int PatientID);
        public int GetAllAppointementsCount(int PatientID, DateTime? date = null);
        public List<Appointement>? GetAllAppointements(int PatientID, DateTime? date = null, int pageNumber = 1, int pageSize = 10);
        public HttpStatusCode AddPatient(Patient patient);
        public HttpStatusCode EditPatient(Patient patient);
        public HttpStatusCode DeletePatient(int id);
        public HttpStatusCode AddAppointment(Appointement app);
        public HttpStatusCode CancelAppointment(int AppID);
        public HttpStatusCode AddCard(Paycard card);
        public HttpStatusCode EditCard(Paycard card);
        public HttpStatusCode DeleteCard(int cardID);
        public Paycard? GetCard(int patientID);
        public HttpStatusCode AddReview(Review review);
        public HttpStatusCode EditReview(Review review);
        public HttpStatusCode DeleteReview(int reviewID);
        public Review? GetReview(int reviewID);
        public List<AppWithDoc> GetAllApps(int patientID);

    }
}
