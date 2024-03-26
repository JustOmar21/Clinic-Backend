﻿using Clinic.Core.DTO;
using Clinic.Core.Models;
using Clinic.Core.Repos;
using Clinic.Infrastructure.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            Doctor? doc = context.Doctors.SingleOrDefault(doc => doc.Id == app.DoctorID);
            if (doc == null) { throw new KeyNotFoundException($"Doctor with ID {app.DoctorID} does not exist"); }
            Patient? patient = context.Patients.SingleOrDefault(pat=>pat.Id == app.PatientID);
            if (patient == null) { throw new KeyNotFoundException($"Patient with ID {app.PatientID} does not exist"); }
            context.Appointements.Add(app);
            context.SaveChanges();
            string gender = patient.Gender == Gender.PreferNotToSay ? "" : (patient.Gender == Gender.Male ? "Mr." : "Mrs.");
            EmailUtilities.SendEmail("Appointment Requested", $"Dear {doc.Name}\nWe would like to inform you that {gender}{patient.Name} requested an appointment to be scheduled on {app.Date.Date.ToLongDateString()}\nPlease provide a response at <put link or mention appointment page>");
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
                EmailUtilities.SendEmail("Appointment Canceled", $"Dear {findApp.Doctor.Name}\nWe regret to inform you that {gender}{findApp.Patient.Name} cancaled the appointment scheduled on {findApp.Date.ToLongDateString()}.");
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
                Patient? patient = context.Patients.SingleOrDefault(pat => pat.Id == card.PatientID);
                if (patient == null) { throw new KeyNotFoundException($"Patient with ID {card.PatientID} does not exist"); }
                context.Paycard.Add(card);
                context.SaveChanges();
                return HttpStatusCode.Created;
            }
            else { return HttpStatusCode.BadRequest; }
        }


        public HttpStatusCode AddPatient(Patient patient)
        {
            if(patient != null)
            {
                context.Patients.Add(patient);
                context.SaveChanges();
                return HttpStatusCode.Created;
            }
            else { return HttpStatusCode.BadRequest; }
        }

        public HttpStatusCode AddReview(Review review)
        {
            if(review != null)
            {
                Doctor? doc = context.Doctors.SingleOrDefault(doc => doc.Id == review.DoctorID);
                if (doc == null) { throw new KeyNotFoundException($"Doctor with ID {review.DoctorID} does not exist"); }
                Patient? patient = context.Patients.SingleOrDefault(pat => pat.Id == review.PatientID);
                if (patient == null) { throw new KeyNotFoundException($"Patient with ID {review.PatientID} does not exist"); }
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
                context.Patients.Remove(patient);
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
                context.Paycard.Update(card);
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else { return HttpStatusCode.NotFound; }
        }
        public Paycard? GetCard(int patientID)
        {
            var card = context.Paycard.SingleOrDefault(card => card.PatientID == patientID);
            if (card is null) { throw new KeyNotFoundException($"Patient with ID {patientID} doesn't have a card"); }
            return card;
        }

        public HttpStatusCode EditPatient(Patient patient)
        {
            var checkPatient = context.Patients.SingleOrDefault(pat => pat.Id == patient.Id);
            if (checkPatient != null)
            {
                context.Patients.Update(patient);
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
                context.Reviews.Update(review);
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else { return HttpStatusCode.NotFound; }
        }

        public List<Appointement>? GetAllAppointements(int PatientID, DateTime? date = null)
        {
            Patient? findPatient = context.Patients.Find(PatientID);
            if (findPatient == null) throw new KeyNotFoundException($"Patient with ID {PatientID} doesn't exist");
            if (date != null)
            {
                return context.Appointements.Where(app => app.Date.Date == date.Value.Date && app.PatientID == PatientID).ToList();
            }
            else
            {
                return context.Appointements.Where(app => app.PatientID == PatientID).ToList();
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

        public SingleDoctorAppointment? GetAppointements(int DoctorID, int PatientID)
        {
            Doctor? doc = context.Doctors.SingleOrDefault(doc => doc.Id == DoctorID);
            if (doc == null) { throw new KeyNotFoundException($"Doctor with ID {DoctorID} does not exist"); }
            Patient? patient = context.Patients.SingleOrDefault(pat => pat.Id == PatientID);
            if (patient == null) { throw new KeyNotFoundException($"Patient with ID {PatientID} does not exist"); ; }

            List<Appointement> app = context.Appointements.Where(app => app.DoctorID == DoctorID && app.PatientID == PatientID).ToList();
            SingleDoctorAppointment patientApps = new SingleDoctorAppointment();
            patientApps.Doctor = doc;
            patientApps.Appointements = app;
            return patientApps;
        }

        public Patient? GetPatient(int id)
        {
            Patient? patient = context.Patients.SingleOrDefault(pat=> pat.Id == id);
            if (patient == null) { throw new KeyNotFoundException($"Patient with ID {id} doesn't exist"); }
            return patient;
        }
        public Review? GetReview(int id)
        {
            Review? review = context.Reviews.SingleOrDefault(review => review.Id == id);
            if (review == null) { throw new KeyNotFoundException($"Review with ID {id} doesn't exist"); }
            return review;
        }

        public int GetPatientCount()
        {
            return context.Patients.Count();
        }
    }
}
