using Clinic.Core.DTO;
using Clinic.Core.Models;
using Clinic.Core.Repos;
using Clinic.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Utilites;

namespace Clinic.Infrastructure.RepoImplemention
{

    public class DoctorRepo : IDoctorRepo
    {
        private readonly ClinicDBContext context;
        public DoctorRepo(ClinicDBContext context)
        {
            this.context = context;
        }
        public HttpStatusCode AddDoctor(Doctor doctor)
        {

            if(doctor != null)
            {
                context.Doctors.Add(doctor);
                context.SaveChanges();
            }
            else
            {
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.NoContent;
            
        }
        public HttpStatusCode DeleteDoctor(int id)
        {
            Doctor? findDoc = context.Doctors.Find(id);
            if(findDoc != null)
            {
                context.Doctors.Remove(findDoc);
                context.SaveChanges();
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
            return HttpStatusCode.NoContent;
        }

        public HttpStatusCode EditDoctor(Doctor doctor)
        {
            Doctor? findDoc = context.Doctors.Find(doctor.Id);
            if (findDoc != null)
            {
                context.Doctors.Update(findDoc);
                context.SaveChanges();
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
            return HttpStatusCode.NoContent;
        }

        public HttpStatusCode CancalAppointment(int AppID)
        {
            Appointement? findApp = context.Appointements.Include(app => app.Patient).SingleOrDefault(app=>app.Id == AppID);
            if (findApp != null)
            {
                findApp.Status = AppStatus.Cancaled;
                context.SaveChanges();
                EmailSender.SendEmail("Appointment Canceled", $"Dear {findApp.Patient.Name}\n We regret to inform you that Dr.{findApp.Doctor.Name} cancaled the appointment scheduled on {findApp.Date.ToLongDateString()}\nPlease retry booking again if needed");
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
            return HttpStatusCode.NoContent;
        }

        public HttpStatusCode ConfirmAppointment(int AppID)
        {
            Appointement? findApp = context.Appointements.Include(app => app.Patient).Include(app => app.Doctor).SingleOrDefault(app => app.Id == AppID);
            if (findApp != null)
            {
                findApp.Status = AppStatus.Accepted;
                context.SaveChanges();
                EmailSender.SendEmail("Appointment Confirmed", $"Dear {findApp.Patient.Name}\n We would like to inform you that Dr.{findApp.Doctor.Name} confirmed the appointment scheduled on {findApp.Date.ToLongDateString()}");
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
            return HttpStatusCode.NoContent;
        }
        public HttpStatusCode RejectAppointment(int AppID)
        {
            Appointement? findApp = context.Appointements.Include(app => app.Patient).SingleOrDefault(app => app.Id == AppID);
            if (findApp != null)
            {
                findApp.Status = AppStatus.Rejected;
                context.SaveChanges();
                EmailSender.SendEmail("Appointment Canceled", $"Dear {findApp.Patient.Name}\n We regret to inform you that Dr.{findApp.Doctor.Name} rejected the appointment scheduled on {findApp.Date.ToLongDateString()}\nPlease retry booking again if needed");
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
            return HttpStatusCode.NoContent;
        }


        public dynamic GetAllAppointements(int DoctorID, DateTime? date = null)
        {
            Doctor? findDoc = context.Doctors.Find(DoctorID);
            if(findDoc != null)
            {
                List<Appointement> apps;
                List<SinglePatientAppointment> patientApps = new List<SinglePatientAppointment>();
                if (date != null)
                {
                    apps = context.Appointements.Where(app=> app.Date.Date == date.Value.Date && app.DoctorID == DoctorID).ToList();
                }
                else
                {
                    apps = context.Appointements.Where(app => app.DoctorID == DoctorID).ToList();
                }
                foreach(Appointement app in apps)
                {
                    SinglePatientAppointment singleApp = new SinglePatientAppointment();
                    singleApp.Patient = context.Patients.SingleOrDefault(pat=>pat.Id == app.PatientID);
                    singleApp.Appointement = app;
                    patientApps.Add(singleApp);
                }
                return patientApps;
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
        }

        public List<SingleDoctorDetails> GetAllDoctors(int pageNumber = 1, int pageSize = 10, string Location = "", int specialityID = -1, string email = "")
        {
            List<Doctor> doctors = context.Doctors.ToList();
            if (Location != "") doctors = doctors.Where(doc => doc.Governance == Location).ToList();
            if (specialityID != -1) doctors = doctors.Where(doc => doc.SpecialityID == specialityID).ToList();
            doctors =
                doctors
                .Where(doc=> doc.Email.Contains(email))
                .Skip((pageSize - 1) * pageNumber)
                .Take(pageSize)
                .ToList();
            List<SingleDoctorDetails> docsFullDetails = new List<SingleDoctorDetails> ();
            foreach(Doctor doc in doctors)
            {
                SingleDoctorDetails doctorDetails = new SingleDoctorDetails();
                doctorDetails.Doctor = doc;
                doctorDetails.Schedule = context.Schedule.SingleOrDefault(sch => sch.DoctorID == doc.Id);
                doctorDetails.Speciality = context.Speciality.SingleOrDefault(spy => spy.ID == doctorDetails.Doctor.SpecialityID);
                doctorDetails.Reviews = context.Reviews.Where(rev => rev.DoctorID == doc.Id).ToList();
                docsFullDetails.Add(doctorDetails);
            }
            return docsFullDetails;
        }

        public dynamic GetDoctor(int id)
        {
            SingleDoctorDetails doctorDetails = new SingleDoctorDetails();
            doctorDetails.Doctor = context.Doctors.SingleOrDefault(doc => doc.Id == id);
            if(doctorDetails.Doctor == null) { return HttpStatusCode.NotFound; }
            doctorDetails.Schedule = context.Schedule.SingleOrDefault(sch => sch.DoctorID == doctorDetails.Doctor.Id);
            doctorDetails.Speciality = context.Speciality.SingleOrDefault(doc => doc.ID == doctorDetails.Doctor.SpecialityID);
            doctorDetails.Reviews = context.Reviews.Where(rev => rev.DoctorID == id).ToList();
            return doctorDetails;
        }

        public int GetDoctorsCount()
        {
            return context.Doctors.Count();
        }

        public dynamic GetPatientAppointements(int DoctorID, int PatientID)
        {
            Doctor? doc = context.Doctors.SingleOrDefault(doc=>doc.Id ==  DoctorID);
            Patient? patient = context.Patients.SingleOrDefault(pat=> pat.Id == PatientID);
            if (doc == null || patient == null) return HttpStatusCode.NotFound;
            List<Appointement> app = context.Appointements.Where(app=>app.DoctorID==DoctorID&&app.PatientID==PatientID).ToList();
            PatientAppointments patientApps = new PatientAppointments();
            patientApps.Patient = patient;
            patientApps.Appointements = app;
            return patientApps;
        }

        public HttpStatusCode AddSchedule(Schedule schedule)
        {
            if(schedule != null)
            {
                context.Schedule.Add(schedule);
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else return HttpStatusCode.BadRequest;
        }

        public HttpStatusCode EditSchedule(Schedule schedule)
        {
            if (schedule != null)
            {
                context.Schedule.Update(schedule);
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else return HttpStatusCode.BadRequest;
        }

        public int GetCurrentOrder(DateTime date)
        {
            return context.Appointements.Where(app=>app.Date.Date == date.Date).Count();
        }
    }
}
