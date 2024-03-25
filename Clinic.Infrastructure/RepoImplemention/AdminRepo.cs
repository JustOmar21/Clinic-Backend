﻿using Clinic.Core.Models;
using Clinic.Core.Repos;
using Clinic.Infrastructure.DBContext;
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
    public class AdminRepo : IAdminRepo
    {
        private readonly ClinicDBContext context;
        public AdminRepo(ClinicDBContext context)
        {
            this.context = context;
        }
        public HttpStatusCode ActivateAccount(int accountID, string accountType = "")
        {
            
            if(accountType == "Doctor" )
            {
                Doctor? doctor = context.Doctors.FirstOrDefault(doc=> doc.Id == accountID);
                if(doctor == null )return HttpStatusCode.BadRequest;
                doctor.Status = Status.Active;
                return HttpStatusCode.NoContent;
            }
            else if(accountType == "Patient")
            {
                Patient? patient = context.Patients.FirstOrDefault(pat => pat.Id == accountID);
                if (patient == null) return HttpStatusCode.BadRequest;
                patient.Status = Status.Active;
                return HttpStatusCode.NoContent;
            }
            return HttpStatusCode.BadRequest;
        }

        public HttpStatusCode BanAccount(int accountID, string accountType)
        {
            if (accountType == "Doctor")
            {
                Doctor? doctor = context.Doctors.FirstOrDefault(doc => doc.Id == accountID);
                if (doctor == null) return HttpStatusCode.BadRequest;
                doctor.Status = Status.Banned;
                return HttpStatusCode.NoContent;
            }
            else if (accountType == "Patient")
            {
                Patient? patient = context.Patients.FirstOrDefault(pat => pat.Id == accountID);
                if (patient == null) return HttpStatusCode.BadRequest;
                patient.Status = Status.Banned;
                return HttpStatusCode.NoContent;
            }
            return HttpStatusCode.BadRequest;
        }

        public HttpStatusCode ConfirmationRequest(int accountID, string accountType)
        {
            if (accountType == "Doctor")
            {
                Doctor? doctor = context.Doctors.FirstOrDefault(doc => doc.Id == accountID);
                if (doctor == null) return HttpStatusCode.BadRequest;
                confirmEmail? checkConfirm = context.ConfirmEmail.SingleOrDefault(con=>con.Email == doctor.Email);
                checkConfirm = checkConfirm == null ? new confirmEmail() : checkConfirm;
                checkConfirm.Email = doctor.Email;
                checkConfirm.keypass = EmailUtilities.GenerateKeypass(6);
                context.ConfirmEmail.Update(checkConfirm);
                EmailUtilities.SendEmail("Confirm Account", $"Dear Dr.{doctor.Name}\nWe request that you activate this account\nPassKey: {checkConfirm.keypass} \nPlease enter your keypass at <Link or not we will decide later>");
                return HttpStatusCode.NoContent;
            }
            else if (accountType == "Patient")
            {
                Patient? patient = context.Patients.FirstOrDefault(pat => pat.Id == accountID);
                if (patient == null) return HttpStatusCode.BadRequest;
                confirmEmail? checkConfirm = context.ConfirmEmail.SingleOrDefault(con => con.Email == patient.Email);
                checkConfirm = checkConfirm == null ? new confirmEmail() : checkConfirm;
                checkConfirm.Email = patient.Email;
                checkConfirm.keypass = EmailUtilities.GenerateKeypass(6);
                context.ConfirmEmail.Update(checkConfirm);
                string gender = patient.Gender == Gender.Male ? "Mr." : "Mrs.";
                if (patient.Gender == Gender.PreferNotToSay) gender = "";
                EmailUtilities.SendEmail("Confirm Account", $"Dear {gender}{patient.Name}\nWe request that you activate this account\nPassKey: {checkConfirm.keypass} \nPlease enter your keypass at <Link or not we will decide later>");
                return HttpStatusCode.NoContent;
            }
            return HttpStatusCode.BadRequest;
        }

        public HttpStatusCode DeactivateAccount(int accountID, string accountType)
        {
            if (accountType == "Doctor")
            {
                Doctor? doctor = context.Doctors.FirstOrDefault(doc => doc.Id == accountID);
                if (doctor == null) return HttpStatusCode.BadRequest;
                doctor.Status = Status.Inactive;
                return HttpStatusCode.NoContent;
            }
            else if (accountType == "Patient")
            {
                Patient? patient = context.Patients.FirstOrDefault(pat => pat.Id == accountID);
                if (patient == null) return HttpStatusCode.BadRequest;
                patient.Status = Status.Inactive;
                return HttpStatusCode.NoContent;
            }
            return HttpStatusCode.BadRequest;
        }

        public dynamic GetKeypass(int accountID, string accountType)
        {
            if (accountType == "Doctor")
            {
                Doctor? doctor = context.Doctors.FirstOrDefault(doc => doc.Id == accountID);
                if (doctor == null) return HttpStatusCode.BadRequest;
                confirmEmail? confirm = context.ConfirmEmail.SingleOrDefault(con => con.Email == doctor.Email);
                if(confirm == null) return HttpStatusCode.NotFound;
                return confirm.keypass;
            }
            else if (accountType == "Patient")
            {
                Patient? patient = context.Patients.FirstOrDefault(pat => pat.Id == accountID);
                confirmEmail? confirm = context.ConfirmEmail.SingleOrDefault(con => con.Email == patient.Email);
                if (confirm == null) return HttpStatusCode.NotFound;
                return confirm.keypass;
            }
            return HttpStatusCode.BadRequest;
        }
    }
}
