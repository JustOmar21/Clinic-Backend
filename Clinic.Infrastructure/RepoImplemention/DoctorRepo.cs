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
        public HttpStatusCode AddDoctor(AddDoctor doctorDTO)
        {

            if(doctorDTO.doctor != null)
            {
                context.Logins.Add(new Login { username = doctorDTO.doctor.Email, password = doctorDTO.password, type = "doctor" });
                context.Doctors.Add(doctorDTO.doctor);
                Schedule schedule = new Schedule
                {
                    DoctorID = doctorDTO.doctor.Id,
                    Saturday = "0",
                    Sunday = "0",
                    Monday = "0",
                    Tuesday = "0",
                    Wednesday = "0",
                    Thursday = "0",
                    Friday = "0"
                };
                context.Schedule.Add(schedule);
                context.SaveChanges();
            }
            else
            {
                return HttpStatusCode.BadRequest;
            }
            return HttpStatusCode.Created;
            
        }
        public HttpStatusCode DeleteDoctor(int id)
        {
            Doctor? findDoc = context.Doctors.Find(id);
            if(findDoc != null)
            {
                var log = context.Logins.SingleOrDefault(log => log.username == findDoc.Email);
                context.Doctors.Remove(findDoc);
                context.Logins.Remove(log);
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
                var log = context.Logins.SingleOrDefault(log => log.username == findDoc.Email);
                findDoc.Address = doctor.Address;
                findDoc.AppointmentPrice = doctor.AppointmentPrice;
                findDoc.Status = doctor.Status;
                findDoc.Email = doctor.Email;
                log.username = doctor.Email;
                findDoc.DOB = doctor.DOB;
                findDoc.Name = doctor.Name;
                findDoc.NationalID = doctor.NationalID;
                findDoc.Phone = doctor.Phone;
                findDoc.Governance = doctor.Governance;
                findDoc.SpecialityID = doctor.SpecialityID;
                findDoc.Gender = doctor.Gender;
                context.SaveChanges();
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
            return HttpStatusCode.NoContent;
        }

        public HttpStatusCode CancelAppointment(int AppID)
        {
            Appointement? findApp = context.Appointements.Include(app => app.Patient).SingleOrDefault(app=>app.Id == AppID);
            if (findApp != null)
            {
                if (findApp.Status == AppStatus.Cancaled) return HttpStatusCode.NoContent;
                findApp.Status = AppStatus.Cancaled;
                context.SaveChanges();
                //EmailUtilities.SendEmail("Appointment Canceled", $"Dear {findApp.Patient.Name}\nWe regret to inform you that Dr.{findApp.Doctor.Name} cancaled the appointment scheduled on {findApp.Date.ToLongDateString()}\nPlease retry booking again if needed");
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
                if (findApp.Status == AppStatus.Accepted) return HttpStatusCode.NoContent;
                findApp.Status = AppStatus.Accepted;
                context.SaveChanges();
                //EmailUtilities.SendEmail("Appointment Confirmed", $"Dear {findApp.Patient.Name}\nWe would like to inform you that Dr.{findApp.Doctor.Name} confirmed the appointment scheduled on {findApp.Date.ToLongDateString()}");
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
                if (findApp.Status == AppStatus.Rejected) return HttpStatusCode.NoContent;
                findApp.Status = AppStatus.Rejected;
                context.SaveChanges();
                //EmailUtilities.SendEmail("Appointment Rejected", $"Dear {findApp.Patient.Name}\nWe regret to inform you that Dr.{findApp.Doctor.Name} rejected the appointment scheduled on {findApp.Date.ToLongDateString()}\nPlease retry booking again if needed");
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
            return HttpStatusCode.NoContent;
        }


        public List<SinglePatientAppointment>? GetAllAppointements(int DoctorID, DateTime? date = null, int pageNumber = 1, int pageSize = 10)
        {
            Doctor? findDoc = context.Doctors.Find(DoctorID) ?? throw new KeyNotFoundException($"Doctor with ID {DoctorID} doesn't exist");
            List<Appointement> apps;
            List<SinglePatientAppointment> patientApps = new List<SinglePatientAppointment>();
            if (date != null)
            {
                apps = context.Appointements
                    .Where(app=> app.Date.Date == date.Value.Date && app.DoctorID == DoctorID)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                apps = context.Appointements
                    .Where(app => app.DoctorID == DoctorID)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            foreach(Appointement app in apps)
            {
                SinglePatientAppointment singleApp = new SinglePatientAppointment();
                singleApp.Patient = context.Patients.SingleOrDefault(pat => pat.Id == app.PatientID);
                singleApp.Appointement = app;
                patientApps.Add(singleApp);
            }
            return patientApps;
        }
        public int GetAllAppointementsCount(int DoctorID, DateTime? date = null)
        {
            Doctor? findDoc = context.Doctors.Find(DoctorID) ?? throw new KeyNotFoundException($"Doctor with ID {DoctorID} doesn't exist");
            if (date != null)
            {
                return context.Appointements
                    .Where(app => app.Date.Date == date.Value.Date && app.DoctorID == DoctorID)
                    .Count();
            }
            else
            {
                return context.Appointements
                    .Where(app => app.DoctorID == DoctorID)
                    .Count();
            }
        }

        public List<SingleDoctorDetails> GetAllDoctorsDetails(int pageNumber = 1, int pageSize = 10, string Location = "", int specialityID = -1, string email = "")
        {
            List<Doctor> doctors = context.Doctors.ToList();
            if (Location != "") doctors = doctors.Where(doc => doc.Governance == Location).ToList();
            if (specialityID != -1) doctors = doctors.Where(doc => doc.SpecialityID == specialityID).ToList();
            doctors =
                doctors
                .Where(doc=> doc.Email.Contains(email))
                .Skip((pageNumber - 1) * pageSize)
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
        public List<Doctor> GetAllDoctors(int pageNumber = 1, int pageSize = 10, string Location = "", int specialityID = -1, string email = "")
        {
            List<Doctor> doctors = context.Doctors.ToList();
            if (Location != "") doctors = doctors.Where(doc => doc.Governance == Location).ToList();
            if (specialityID != -1) doctors = doctors.Where(doc => doc.SpecialityID == specialityID).ToList();
            return doctors
                    .Where(doc => doc.Email.Contains(email))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
        }

        public SingleDoctorDetails GetDoctorDetails(int id)
        {
            SingleDoctorDetails doctorDetails = new SingleDoctorDetails();
            doctorDetails.Doctor = context.Doctors.SingleOrDefault(doc => doc.Id == id) ?? throw new KeyNotFoundException($"Doctor with ID {id} doesn't exist");
            doctorDetails.Schedule = context.Schedule.SingleOrDefault(sch => sch.DoctorID == doctorDetails.Doctor.Id);
            doctorDetails.Speciality = context.Speciality.SingleOrDefault(doc => doc.ID == doctorDetails.Doctor.SpecialityID);
            doctorDetails.Reviews = context.Reviews.Where(rev => rev.DoctorID == id).ToList();
            return doctorDetails;
        }
        public Doctor GetDoctor(int id)
        {
            return context.Doctors.SingleOrDefault(doc => doc.Id == id) ?? throw new KeyNotFoundException($"Doctor with ID {id} doesn't exist");
        }

        public int GetDoctorsCount(string Location = "", int specialityID = -1, string email = "")
        {
            List<Doctor> doctors = context.Doctors.ToList();
            if (Location != "") doctors = doctors.Where(doc => doc.Governance == Location).ToList();
            if (specialityID != -1) doctors = doctors.Where(doc => doc.SpecialityID == specialityID).ToList();
            return doctors
                    .Where(doc => doc.Email.Contains(email))
                    .Count();
        }

        public PatientAppointments GetPatientAppointements(int DoctorID, int PatientID, int pageNumber = 1, int pageSize = 10)
        {
            Doctor? doc = context.Doctors.SingleOrDefault(doc=>doc.Id ==  DoctorID) ?? throw new KeyNotFoundException($"Doctor with ID {DoctorID} doesn't exist");
            Patient? patient = context.Patients.SingleOrDefault(pat=> pat.Id == PatientID) ?? throw new KeyNotFoundException($"Patient with ID {PatientID} doesn't exist");
            List<Appointement> app = context.Appointements.Where(app=>app.DoctorID==DoctorID&&app.PatientID==PatientID)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(app=>app.Date)
                .ToList();
            PatientAppointments patientApps = new PatientAppointments
            {
                Patient = patient,
                Appointements = app
            };
            return patientApps;
        }
        public int GetPatientAppointementsCount(int DoctorID, int PatientID)
        {
            Doctor? doc = context.Doctors.SingleOrDefault(doc => doc.Id == DoctorID) ?? throw new KeyNotFoundException($"Doctor with ID {DoctorID} doesn't exist");
            Patient? patient = context.Patients.SingleOrDefault(pat => pat.Id == PatientID) ?? throw new KeyNotFoundException($"Patient with ID {PatientID} doesn't exist");
            return context.Appointements
                .Where(app => app.DoctorID == DoctorID && app.PatientID == PatientID)
                .Count();
        }

        public HttpStatusCode AddSchedule(Schedule schedule)
        {
            
            if (schedule != null)
            {
                Doctor? doc = context.Doctors.SingleOrDefault(doc => doc.Id == schedule.DoctorID) ?? throw new KeyNotFoundException($"Doctor with ID {schedule.DoctorID} does not exist");
                context.Schedule.Add(schedule);
                context.SaveChanges();
                return HttpStatusCode.Created;
            }
            else return HttpStatusCode.BadRequest;
        }

        public HttpStatusCode EditSchedule(Schedule schedule)
        {
            Schedule? checkSchedule = context.Schedule.SingleOrDefault(sch=>sch.Id == schedule.Id);
            if (checkSchedule != null)
            {
                checkSchedule.Thursday = schedule.Thursday;
                checkSchedule.Friday = schedule.Friday;
                checkSchedule.Saturday = schedule.Saturday;
                checkSchedule.Sunday = schedule.Sunday;
                checkSchedule.Monday = schedule.Monday;
                checkSchedule.Tuesday = schedule.Tuesday;
                checkSchedule.Wednesday = schedule.Wednesday;
                checkSchedule.DoctorID = schedule.DoctorID;
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else return HttpStatusCode.NotFound;
        }
        public Schedule GetSchedule(int docID)
        {
            return context.Schedule.SingleOrDefault(scd => scd.DoctorID == docID) ?? throw new KeyNotFoundException($"This Doctor with ID {docID} does not have a schedule");
        }
        public HttpStatusCode AddSpeciality(Speciality speciality)
        {

            if (speciality != null)
            {
                context.Speciality.Add(speciality);
                context.SaveChanges();
                return HttpStatusCode.Created;
            }
            else return HttpStatusCode.BadRequest;
        }

        public HttpStatusCode EditSpeciality(Speciality speciality)
        {
            Speciality checkSpec = context.Speciality.SingleOrDefault(spec => spec.ID == speciality.ID) ?? throw new KeyNotFoundException($"Speciality with ID {speciality.ID} doesn't exist");
            if (checkSpec != null)
            {
                checkSpec.Name = speciality.Name;
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else return HttpStatusCode.BadRequest;
        }
        public HttpStatusCode DeleteSpeciality(int specialityID)
        {
            Speciality checkSpec = context.Speciality.SingleOrDefault(spec => spec.ID == specialityID) ?? throw new KeyNotFoundException($"Speciality with ID {specialityID} doesn't exist");
            if (checkSpec != null)
            {
                context.Speciality.Remove(checkSpec);
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else return HttpStatusCode.BadRequest;
        }
        public Speciality GetSpeciality(int specialityID)
        {
            return context.Speciality.SingleOrDefault(spc => spc.ID == specialityID) ?? throw new KeyNotFoundException($"Speciality with ID {specialityID} doesn't exist");
        }

        public List<Speciality> GetAllSpeciality()
        {
            return context.Speciality.ToList();
        }

        public int GetCurrentOrder(DateTime date)
        {
            return context.Appointements.Where(app=>app.Date.Date == date.Date).Count();
        }
        public List<SingleDoctor> GetDoctorsWithSpec()
        {
            List<Doctor>? doctors = context.Doctors.ToList();
            List<SingleDoctor> singleDoctors = new List<SingleDoctor>();
            foreach(Doctor doc in doctors)
            {
                SingleDoctor docs = new SingleDoctor
                {
                    Doctor = doc,
                    Speciality = context.Speciality.SingleOrDefault(spec => spec.ID == doc.SpecialityID)
                };
                singleDoctors.Add(docs);
            }
            return singleDoctors;
        }
        public List<SinglePatientAppointment> GetAllRequestedAppointments(int DoctorID, DateTime? date)
        {
            Doctor? findDoc = context.Doctors.Find(DoctorID) ?? throw new KeyNotFoundException($"Doctor with ID {DoctorID} doesn't exist");
            List<Appointement> apps;
            List<SinglePatientAppointment> patientApps = new List<SinglePatientAppointment>();
            if (date != null)
            {
                apps = context.Appointements
                    .Where(app => app.Date.Date == date.Value.Date && app.DoctorID == DoctorID && app.Status == AppStatus.Pending)
                    .OrderByDescending(app => app.Date)
                    .ToList();
            }
            else
            {
                apps = context.Appointements
                    .Where(app => app.DoctorID == DoctorID && app.Status == AppStatus.Pending)
                    .OrderByDescending(app => app.Date)
                    .ToList();
            }
            foreach (Appointement app in apps)
            {
                SinglePatientAppointment singleApp = new SinglePatientAppointment();
                singleApp.Patient = context.Patients.SingleOrDefault(pat => pat.Id == app.PatientID);
                singleApp.Appointement = app;
                patientApps.Add(singleApp);
            }
            return patientApps;
        }
        public List<SinglePatientAppointment> GetAllOtherAppointments(int DoctorID, DateTime? date)
        {
            Doctor? findDoc = context.Doctors.Find(DoctorID) ?? throw new KeyNotFoundException($"Doctor with ID {DoctorID} doesn't exist");
            List<Appointement> apps;
            List<SinglePatientAppointment> patientApps = new List<SinglePatientAppointment>();
            if (date != null)
            {
                apps = context.Appointements
                    .Where(app => app.Date.Date == date.Value.Date && app.DoctorID == DoctorID && app.Status != AppStatus.Pending)
                    .OrderByDescending(app => app.Date)
                    .ToList();
            }
            else
            {
                apps = context.Appointements
                    .Where(app => app.DoctorID == DoctorID && app.Status != AppStatus.Pending)
                    .OrderByDescending(app => app.Date)
                    .ToList();
            }
            foreach (Appointement app in apps)
            {
                SinglePatientAppointment singleApp = new SinglePatientAppointment();
                singleApp.Patient = context.Patients.SingleOrDefault(pat => pat.Id == app.PatientID);
                singleApp.Appointement = app;
                patientApps.Add(singleApp);
            }
            return patientApps;
        }

        public List<SinglePatientAppointment> GetAllAcceptedAppointments(int DoctorID, DateTime? date)
        {
            Doctor? findDoc = context.Doctors.Find(DoctorID) ?? throw new KeyNotFoundException($"Doctor with ID {DoctorID} doesn't exist");
            List<Appointement> apps;
            List<SinglePatientAppointment> patientApps = new List<SinglePatientAppointment>();
            if (date != null)
            {
                apps = context.Appointements
                    .Where(app => app.Date.Date == date.Value.Date && app.DoctorID == DoctorID && app.Status == AppStatus.Accepted)
                    .OrderByDescending(app=>app.Date)
                    .ToList();
            }
            else
            {
                apps = context.Appointements
                    .Where(app => app.DoctorID == DoctorID && app.Status == AppStatus.Accepted)
                    .OrderByDescending(app => app.Date)
                    .ToList();
            }
            foreach (Appointement app in apps)
            {
                SinglePatientAppointment singleApp = new SinglePatientAppointment();
                singleApp.Patient = context.Patients.SingleOrDefault(pat => pat.Id == app.PatientID);
                singleApp.Appointement = app;
                patientApps.Add(singleApp);
            }
            return patientApps;
        }
    }
}
