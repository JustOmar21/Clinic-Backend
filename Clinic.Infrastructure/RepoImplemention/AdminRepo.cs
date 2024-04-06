using Clinic.Core.DTO;
using Clinic.Core.Models;
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
        public HttpStatusCode ActivateAccount(AccountStatus account)
        {
            
            if(account.Type == "Doctor" )
            {
                Doctor? doctor = context.Doctors.FirstOrDefault(doc=> doc.Id == account.ID);
                if(doctor == null )return HttpStatusCode.NotFound;
                doctor.Status = Status.Active;
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else if(account.Type == "Patient")
            {
                Patient? patient = context.Patients.FirstOrDefault(pat => pat.Id == account.ID);
                if (patient == null) return HttpStatusCode.NotFound;
                patient.Status = Status.Active;
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            throw new KeyNotFoundException($"Account Type {account.Type} does not exist");
        }
        public HttpStatusCode RejectAccount(AccountStatus account)
        {
            if (account.Type == "Doctor")
            {
                Doctor? doctor = context.Doctors.FirstOrDefault(doc => doc.Id == account.ID);
                if (doctor == null) return HttpStatusCode.NotFound;
                doctor.Status = Status.Rejected;
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else if (account.Type == "Patient")
            {
                Patient? patient = context.Patients.FirstOrDefault(pat => pat.Id == account.ID);
                if (patient == null) return HttpStatusCode.NotFound;
                patient.Status = Status.Rejected;
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            throw new KeyNotFoundException($"Account Type {account.Type} does not exist");
        }

        public HttpStatusCode BanAccount(AccountStatus account)
        {
            if (account.Type == "Doctor")
            {
                Doctor? doctor = context.Doctors.FirstOrDefault(doc => doc.Id == account.ID);
                if (doctor == null) return HttpStatusCode.NotFound;
                doctor.Status = Status.Banned;
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else if (account.Type == "Patient")
            {
                Patient? patient = context.Patients.FirstOrDefault(pat => pat.Id == account.ID);
                if (patient == null) return HttpStatusCode.NotFound;
                patient.Status = Status.Banned;
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            throw new KeyNotFoundException($"Account Type {account.Type} does not exist");
        }

        public HttpStatusCode ConfirmationRequest(AccountStatus account)
        {
            if (account.Type == "Doctor")
            {
                Doctor? doctor = context.Doctors.FirstOrDefault(doc => doc.Id == account.ID);
                if (doctor == null) return HttpStatusCode.NotFound;
                confirmEmail? checkConfirm = context.ConfirmEmail.SingleOrDefault(con=>con.Email == doctor.Email);
                checkConfirm = checkConfirm == null ? new confirmEmail() : checkConfirm;
                checkConfirm.Email = doctor.Email;
                checkConfirm.keypass = EmailUtilities.GenerateKeypass(6);
                context.ConfirmEmail.Update(checkConfirm);
                EmailUtilities.SendEmail("Confirm Account", $"Dear Dr.{doctor.Name}\nWe request that you activate this account\nPassKey: {checkConfirm.keypass} \nPlease enter your keypass at <Link or not we will decide later>");
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else if (account.Type == "Patient")
            {
                Patient? patient = context.Patients.FirstOrDefault(pat => pat.Id == account.ID);
                if (patient == null) return HttpStatusCode.NotFound;
                confirmEmail? checkConfirm = context.ConfirmEmail.SingleOrDefault(con => con.Email == patient.Email);
                checkConfirm = checkConfirm == null ? new confirmEmail() : checkConfirm;
                checkConfirm.Email = patient.Email;
                checkConfirm.keypass = EmailUtilities.GenerateKeypass(6);
                context.ConfirmEmail.Update(checkConfirm);
                string gender = patient.Gender == Gender.Male ? "Mr." : "Mrs.";
                if (patient.Gender == Gender.PreferNotToSay) gender = "";
                EmailUtilities.SendEmail("Confirm Account", $"Dear {gender}{patient.Name}\nWe request that you activate this account\nPassKey: {checkConfirm.keypass} \nPlease enter your keypass at <Link or not we will decide later>");
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            throw new KeyNotFoundException($"Account Type {account.Type} does not exist");
        }

        public HttpStatusCode DeactivateAccount(AccountStatus account)
        {
            if (account.Type == "Doctor")
            {
                Doctor? doctor = context.Doctors.FirstOrDefault(doc => doc.Id == account.ID);
                if (doctor == null) return HttpStatusCode.BadRequest;
                doctor.Status = Status.Inactive;
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else if (account.Type == "Patient")
            {
                Patient? patient = context.Patients.FirstOrDefault(pat => pat.Id == account.ID);
                if (patient == null) return HttpStatusCode.BadRequest;
                patient.Status = Status.Inactive;
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            throw new KeyNotFoundException($"Account Type {account.Type} does not exist");
        }

        public string GetKeypass(AccountStatus account)
        {
            if (account.Type == "Doctor")
            {
                Doctor? doctor = context.Doctors.SingleOrDefault(doc => doc.Id == account.ID) ?? throw new KeyNotFoundException($"Doctor with ID {account.ID} does not exist");
                confirmEmail? confirm = context.ConfirmEmail.SingleOrDefault(con => con.Email == doctor.Email) ?? throw new KeyNotFoundException($"Doctor with ID {doctor.Id} does not have a keypass");
                return confirm.keypass;
            }
            else if (account.Type == "Patient")
            {
                Patient? patient = context.Patients.FirstOrDefault(pat => pat.Id == account.ID) ?? throw new KeyNotFoundException($"Patient with ID {account.ID} does not exist");
                confirmEmail? confirm = context.ConfirmEmail.SingleOrDefault(con => con.Email == patient.Email) ?? throw new KeyNotFoundException($"Patient with ID {patient.Id} does not have a keypass");
                return confirm.keypass;
            }
            throw new KeyNotFoundException($"Account Type {account.Type} does not exist");
        }
    }
}
