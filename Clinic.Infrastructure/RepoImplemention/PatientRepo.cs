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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utilites;

namespace Clinic.Infrastructure.RepoImplemention
{
    public class PatientRepo : IPatientRepo
    {
        private readonly ClinicDBContext context;
        public PatientRepo(ClinicDBContext context)
        {
            this.context = context;
        }
        public HttpStatusCode AddAppointment(Appointement app)
        {
            Doctor? doc = context.Doctors.SingleOrDefault(doc => doc.Id == app.DoctorID) ?? throw new KeyNotFoundException($"Doctor with ID {app.DoctorID} does not exist");
            Patient? patient = context.Patients.SingleOrDefault(pat=>pat.Id == app.PatientID) ?? throw new KeyNotFoundException($"Patient with ID {app.PatientID} does not exist");
            var findApp = context.Appointements.SingleOrDefault(apps=> apps.DoctorID == doc.Id && apps.PatientID == patient.Id && apps.Date == app.Date);
            if (findApp != null) throw new Exception("You cannot book more than one appointment per day for the same doctor");
            context.Appointements.Add(app);
            context.SaveChanges();
            string gender = patient.Gender == Gender.PreferNotToSay ? "" : (patient.Gender == Gender.Male ? "Mr." : "Mrs.");
            //EmailUtilities.SendEmail("Appointment Requested", $"Dear {doc.Name}\nWe would like to inform you that {gender}{patient.Name} requested an appointment to be scheduled on {app.Date.Date.ToLongDateString()}\nPlease provide a response at <put link or mention appointment page>");
            return HttpStatusCode.Created;
        }
        public HttpStatusCode CancelAppointment(int AppID)
        {
            Appointement? findApp = context.Appointements.Include(app=>app.Doctor).Include(app=>app.Patient).SingleOrDefault(app => app.Id == AppID);
            if (findApp != null)
            {
                if (findApp.Status == AppStatus.Cancaled) return HttpStatusCode.NoContent;
                string gender = findApp.Patient.Gender == Gender.PreferNotToSay ? "" : (findApp.Patient.Gender == Gender.Male ? "Mr." : "Mrs.");
                findApp.Status = AppStatus.Cancaled;
                context.SaveChanges();
                //EmailUtilities.SendEmail("Appointment Canceled", $"Dear {findApp.Doctor.Name}\nWe regret to inform you that {gender}{findApp.Patient.Name} cancaled the appointment scheduled on {findApp.Date.ToLongDateString()}.");
                return HttpStatusCode.NoContent;
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
        }


        public HttpStatusCode AddCard(Paycard card)
        {
            if(card != null)
            {
                Patient? patient = context.Patients.SingleOrDefault(pat => pat.Id == card.PatientID) ?? throw new KeyNotFoundException($"Patient with ID {card.PatientID} does not exist");
                context.Paycard.Add(card);
                context.SaveChanges();
                return HttpStatusCode.Created;
            }
            else { return HttpStatusCode.BadRequest; }
        }


        public HttpStatusCode AddPatient(AddPatient patientDTO)
        {
            if(patientDTO.patient != null)
            {
                var duplicateEmail = context.Doctors.SingleOrDefault(x => x.Email == patientDTO.patient.Email);
                if (duplicateEmail != null) { throw new Exception("The Email you entered is duplicated"); }
                context.Logins.Add(new Login { username = patientDTO.patient.Email, password = patientDTO.password, type = "patient" });
                context.Patients.Add(patientDTO.patient);
                context.SaveChanges();
                return HttpStatusCode.Created;
            }
            else { return HttpStatusCode.BadRequest; }
        }

        public HttpStatusCode AddReview(Review review)
        {
            if(review != null)
            {
                Doctor? doc = context.Doctors.SingleOrDefault(doc => doc.Id == review.DoctorID) ?? throw new KeyNotFoundException($"Doctor with ID {review.DoctorID} does not exist");
                Patient? patient = context.Patients.SingleOrDefault(pat => pat.Id == review.PatientID) ?? throw new KeyNotFoundException($"Patient with ID {review.PatientID} does not exist");
                context.Reviews.Add(review);
                context.SaveChanges();
                return HttpStatusCode.Created;
            }
            else { return HttpStatusCode.BadRequest; }
        }

        public HttpStatusCode DeleteCard(int cardID)
        {
            Paycard? card = context.Paycard.SingleOrDefault(paycard=>paycard.Id == cardID);
            if (card != null)
            {
                context.Paycard.Remove(card);
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else { return HttpStatusCode.NotFound; }
        }

        public HttpStatusCode DeletePatient(int id)
        {
            Patient? patient = context.Patients.SingleOrDefault(pat => pat.Id == id);
            if(patient != null)
            {
                var log = context.Logins.SingleOrDefault(log => log.username == patient.Email);
                context.Patients.Remove(patient);
                context.Logins.Remove(log);
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else { return HttpStatusCode.NotFound; }
        }

        public HttpStatusCode DeleteReview(int reviewID)
        {
            Review? review = context.Reviews.SingleOrDefault(rev=>rev.Id == reviewID);
            if (review != null)
            {
                context.Reviews.Remove(review);
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else { return HttpStatusCode.NotFound; }
        }

        public HttpStatusCode EditCard(Paycard card)
        {
            var cardCheck = context.Paycard.SingleOrDefault(crd => crd.Id == card.Id);
            if (cardCheck != null)
            {
                cardCheck.CCV = card.CCV;
                cardCheck.PatientID = card.PatientID;
                cardCheck.ExpirationDate = card.ExpirationDate;
                cardCheck.Number = card.Number;
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else { return HttpStatusCode.NotFound; }
        }
        public Paycard? GetCard(int patientID)
        {
            return context.Paycard.SingleOrDefault(card => card.PatientID == patientID) ?? throw new KeyNotFoundException($"Patient with ID {patientID} doesn't have a card");
        }

        public HttpStatusCode EditPatient(Patient patient)
        {
            var checkPatient = context.Patients.SingleOrDefault(pat => pat.Id == patient.Id);
            if (checkPatient != null)
            {
                var duplicateEmail = context.Logins.SingleOrDefault(x => x.username != checkPatient.Email && x.username == patient.Email);
                if (duplicateEmail != null) { throw new Exception("The Email you entered is duplicated"); }
                var log = context.Logins.SingleOrDefault(log => log.username == checkPatient.Email);
                checkPatient.Name = patient.Name;
                checkPatient.Status = patient.Status;
                checkPatient.Email = patient.Email;
                log.username = patient.Email;
                checkPatient.Phone = patient.Phone;
                checkPatient.DOB = patient.DOB;
                checkPatient.Gender = patient.Gender;
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else { return HttpStatusCode.NotFound; }
        }

        public HttpStatusCode EditReview(Review review)
        {
            var checkReview = context.Reviews.SingleOrDefault(rev => rev.Id == review.Id);
            if (checkReview != null)
            {
                checkReview.Notes = review.Notes;
                checkReview.Score = review.Score;
                checkReview.PatientID = review.PatientID;
                checkReview.DoctorID = review.DoctorID;
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else { return HttpStatusCode.NotFound; }
        }

        public List<Appointement>? GetAllAppointements(int PatientID, DateTime? date = null, int pageNumber = 1, int pageSize = 10)
        {
            Patient? findPatient = context.Patients.Find(PatientID) ?? throw new KeyNotFoundException($"Patient with ID {PatientID} doesn't exist");
            if (date != null)
            {
                return context.Appointements
                    .Where(app => app.Date.Date == date.Value.Date && app.PatientID == PatientID)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                return context.Appointements.Where(app => app.PatientID == PatientID)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
        }
        public int GetAllAppointementsCount(int PatientID, DateTime? date = null)
        {
            Patient? findPatient = context.Patients.Find(PatientID) ?? throw new KeyNotFoundException($"Patient with ID {PatientID} doesn't exist");
            if (date != null)
            {
                return context.Appointements
                    .Where(app => app.Date.Date == date.Value.Date && app.PatientID == PatientID)
                    .Count();
            }
            else
            {
                return context.Appointements
                    .Where(app => app.PatientID == PatientID)
                    .Count();
            }
        }

        public List<Patient> GetAllPatient(int pageNumber = 1, int pageSize = 10, string name = "", string email = "")
        {
            return context.Patients
                    .Where(pat => pat.Email.Contains(email)&&pat.Name.Contains(name))
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
        }

        public SingleDoctorAppointment? GetAppointements(int DoctorID, int PatientID, int pageNumber = 1, int pageSize = 10)
        {
            Doctor? doc = context.Doctors.SingleOrDefault(doc => doc.Id == DoctorID) ?? throw new KeyNotFoundException($"Doctor with ID {DoctorID} does not exist");
            Patient? patient = context.Patients.SingleOrDefault(pat => pat.Id == PatientID) ?? throw new KeyNotFoundException($"Patient with ID {PatientID} does not exist");

            List<Appointement> app = context.Appointements
                .Where(app => app.DoctorID == DoctorID && app.PatientID == PatientID)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            SingleDoctorAppointment patientApps = new SingleDoctorAppointment();
            patientApps.Doctor = doc;
            patientApps.Appointements = app;
            return patientApps;
        }
        public int GetAppointementsCount(int DoctorID, int PatientID)
        {
            Doctor? doc = context.Doctors.SingleOrDefault(doc => doc.Id == DoctorID) ?? throw new KeyNotFoundException($"Doctor with ID {DoctorID} does not exist");
            Patient? patient = context.Patients.SingleOrDefault(pat => pat.Id == PatientID) ?? throw new KeyNotFoundException($"Patient with ID {PatientID} does not exist");

            return context.Appointements
                .Where(app => app.DoctorID == DoctorID && app.PatientID == PatientID)
                .Count();
        }

        public Patient? GetPatient(int id)
        {
            return context.Patients.SingleOrDefault(pat=> pat.Id == id) ?? throw new KeyNotFoundException($"Patient with ID {id} doesn't exist");
        }
        public Review? GetReview(int id)
        {
            return context.Reviews.SingleOrDefault(review => review.Id == id) ?? throw new KeyNotFoundException($"Review with ID {id} doesn't exist");
        }

        public int GetPatientCount(string name = "", string email = "")
        {
            return context.Patients
                .Where(pat => pat.Email.Contains(email) && pat.Name.Contains(name))
                .Count();
        }

        public List<Patient> GetAllPatientNoPage()
        {
            return context.Patients.ToList();
        }
        public List<AppWithDoc> GetAllApps(int patientID)
        {
            var findPat = context.Patients.SingleOrDefault(pat => pat.Id == patientID) ?? throw new KeyNotFoundException($"Patient with ID {patientID} does not exist");
            var apps = context.Appointements.Where(app=> app.PatientID==patientID).OrderByDescending(app=> app.Date).ToList();
            List<AppWithDoc> docs = new List<AppWithDoc>();
            foreach(var app in apps)
            {
                AppWithDoc appWithDoc = new AppWithDoc();
                appWithDoc.Appointement = app;
                appWithDoc.Doctor = context.Doctors.SingleOrDefault(appss=>appss.Id == app.DoctorID) ?? throw new KeyNotFoundException($"Doctor with ID {app.DoctorID} does not exist");
                docs.Add(appWithDoc);
            }
            return docs;
        }
    }
}
