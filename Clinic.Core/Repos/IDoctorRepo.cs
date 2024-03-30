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
    public interface IDoctorRepo
    {
        public int GetDoctorsCount(string Location = "", int specialityID = -1, string email = "");
        public List<SingleDoctor> GetDoctorsWithSpec();
        public List<Doctor> GetAllDoctors(int pageNumber = 1,int pageSize = 10 ,string Location = "" , int specialityID = -1 , string email = "");
        public List<SingleDoctorDetails> GetAllDoctorsDetails(int pageNumber = 1,int pageSize = 10 ,string Location = "" , int specialityID = -1 , string email = "");
        public Doctor GetDoctor(int id);
        public SingleDoctorDetails GetDoctorDetails(int id);
        public List<SinglePatientAppointment>? GetAllAppointements(int DoctorID, DateTime? date = null, int pageNumber = 1, int pageSize = 10);
        public int GetAllAppointementsCount(int DoctorID, DateTime? date = null);
        public PatientAppointments GetPatientAppointements(int DoctorID, int PatientID, int pageNumber = 1, int pageSize = 10);
        public int GetPatientAppointementsCount(int DoctorID, int PatientID);
        public HttpStatusCode AddDoctor(AddDoctor doctor);
        public HttpStatusCode EditDoctor(Doctor doctor);
        public HttpStatusCode DeleteDoctor(int id);
        public HttpStatusCode ConfirmAppointment(int AppID);
        public HttpStatusCode CancelAppointment(int AppID);
        public HttpStatusCode RejectAppointment(int AppID);
        public HttpStatusCode AddSpeciality (Speciality speciality);
        public HttpStatusCode DeleteSpeciality (int specialityID);
        public HttpStatusCode EditSpeciality (Speciality speciality);
        public Speciality GetSpeciality(int specialityID);
        public List<Speciality> GetAllSpeciality();
        public HttpStatusCode AddSchedule(Schedule schedule);
        public HttpStatusCode EditSchedule(Schedule schedule);
        public Schedule GetSchedule(int scheduleID);
        public int GetCurrentOrder(DateTime date);
        public List<SinglePatientAppointment> GetAllRequestedAppointments(int doctorID , DateTime? date);
        public List<SinglePatientAppointment> GetAllOtherAppointments(int DoctorID, DateTime? date);
        public List<SinglePatientAppointment> GetAllAcceptedAppointments(int DoctorID, DateTime? date);
    }
}
