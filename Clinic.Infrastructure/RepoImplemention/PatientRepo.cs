using Clinic.Core.DTO;
using Clinic.Core.Models;
using Clinic.Core.Repos;
using Clinic.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

            if(app != null)
            {
                context.Appointements.Add(app);
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else { return HttpStatusCode.BadRequest; }
        }
        public HttpStatusCode CancelAppointment(int AppID)
        {
            Appointement? findApp = context.Appointements.SingleOrDefault(app => app.Id == AppID);
            if (findApp != null)
            {
                findApp.Status = AppStatus.Cancaled;
                context.SaveChanges();            }
            else
            {
                return HttpStatusCode.NotFound;
            }
            return HttpStatusCode.NoContent;
        }


        public HttpStatusCode AddCard(Paycard card)
        {
            if(card != null)
            {
                context.Paycard.Add(card);
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else { return HttpStatusCode.BadRequest; }
        }

        public HttpStatusCode AddPatient(Patient patient)
        {
            if(patient != null)
            {
                context.Patients.Add(patient);
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else { return HttpStatusCode.BadRequest; }
        }

        public HttpStatusCode AddReview(Review review)
        {
            if(review != null)
            {
                context.Reviews.Add(review);
                context.SaveChanges();
                return HttpStatusCode.NoContent;
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
            else { return HttpStatusCode.BadRequest; }
        }

        public HttpStatusCode DeletePatient(int id)
        {
            Patient? patient = context.Patients.SingleOrDefault(pat => pat.Id == id);
            if(patient != null)
            {
                context.Patients.Remove(patient);
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else { return HttpStatusCode.BadRequest; }
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
            else { return HttpStatusCode.BadRequest; }
        }

        public HttpStatusCode EditCard(Paycard card)
        {
            if (card != null)
            {
                context.Paycard.Update(card);
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else { return HttpStatusCode.BadRequest; }
        }

        public HttpStatusCode EditPatient(Patient patient)
        {
            if (patient != null)
            {
                context.Patients.Update(patient);
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else { return HttpStatusCode.BadRequest; }
        }

        public HttpStatusCode EditReview(Review review)
        {
            if (review != null)
            {
                context.Reviews.Update(review);
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else { return HttpStatusCode.BadRequest; }
        }

        public dynamic GetAllAppointements(int PatientID, DateTime? date = null)
        {
            Patient? findPatient = context.Patients.Find(PatientID);
            if (findPatient != null)
            {
                if (date != null)
                {
                    return context.Appointements.Where(app => app.Date.Date == date.Value.Date && app.PatientID == PatientID).ToList();
                }
                else
                {
                    return context.Appointements.Where(app => app.PatientID == PatientID).ToList();
                }
            }
            else
            {
                return HttpStatusCode.NotFound;
            }
        }

        public List<Patient> GetAllPatient(int pageNumber = 1, int pageSize = 10, string name = "", string email = "")
        {
            return context.Patients
                    .Where(pat => pat.Email.Contains(email)&&pat.Name.Contains(name))
                    .Skip((pageSize - 1) * pageNumber)
                    .Take(pageSize)
                    .ToList();
        }

        public dynamic GetAppointements(int DoctorID, int PatientID)
        {
            Doctor? doc = context.Doctors.SingleOrDefault(doc => doc.Id == DoctorID);
            Patient? patient = context.Patients.SingleOrDefault(pat => pat.Id == PatientID);
            string message = "";
            if (doc == null || patient == null) return HttpStatusCode.NotFound;
            List<Appointement> app = context.Appointements.Where(app => app.DoctorID == DoctorID && app.PatientID == PatientID).ToList();
            SingleDoctorAppointment patientApps = new SingleDoctorAppointment();
            patientApps.Doctor = doc;
            patientApps.Appointements = app;
            return patientApps;
        }

        public dynamic GetPatient(int id)
        {
            Patient? patient = context.Patients.SingleOrDefault(pat=> pat.Id == id);
            if(patient == null) return HttpStatusCode.NotFound;
            return patient;
        }

        public int GetPatientCount()
        {
            return context.Patients.Count();
        }
    }
}
