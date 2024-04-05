using Clinic.Core.DTO;
using Clinic.Core.Models;
using Clinic.Core.Repos;
using Clinic.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Clinic.Infrastructure.RepoImplemention
{
    public class UserRepo : IUserRepo
    {
        private readonly ClinicDBContext context;
        public UserRepo(ClinicDBContext context)
        {
            this.context = context;
        }
        public UserInfo Login(LoginDTO log)
        {
            Login checkLogin = context.Logins.SingleOrDefault(logs => logs.username == log.email && logs.password == log.password) ?? throw new KeyNotFoundException("Either the email or the password is incorrect");
            if (checkLogin.type == "doctor")
            {
                var doctor = context.Doctors.SingleOrDefault(user => user.Email == checkLogin.username) ?? throw new KeyNotFoundException("This user exist in the login tables but not in the doctors table");
                return new UserInfo() { id = doctor.Id , role = "doctor"};
            }
            else if (checkLogin.type == "patient")
            {
                var patient = context.Patients.SingleOrDefault(user => user.Email == checkLogin.username) ?? throw new KeyNotFoundException("This user exist in the login tables but not in the patient table");
                return new UserInfo() { id = patient.Id, role = "patient" };
            }
            else if (checkLogin.type == "admin")
            {
                return new UserInfo() { id = null, role = "admin" }; ;
            }
            throw new KeyNotFoundException($"The user type '{checkLogin.type}' doesn't exist");
        }
        public HttpStatusCode changePassword(ChangePassword changePassword)
        {
            string userEmail;
            if (changePassword.password == changePassword.newPassword) throw new Exception("New Password cannot be the same as old password");
            if(changePassword.userRole == "patient")
            {
                userEmail = context.Patients.Where(pat => pat.Id == changePassword.userID).Select(pat=>pat.Email).SingleOrDefault() ?? throw new KeyNotFoundException($"Patient with ID {changePassword.userID} doesn't exist");
                var loginData = context.Logins.SingleOrDefault(log => log.username == userEmail && log.password == changePassword.password) ?? throw new KeyNotFoundException("The password entered is incorrect");
                loginData.password = changePassword.newPassword;
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else if (changePassword.userRole == "doctor")
            {
                userEmail = context.Doctors.Where(doc => doc.Id == changePassword.userID).Select(doc => doc.Email).SingleOrDefault() ?? throw new KeyNotFoundException($"Patient with ID {changePassword.userID} doesn't exist");
                var loginData = context.Logins.SingleOrDefault(log => log.username == userEmail && log.password == changePassword.password) ?? throw new KeyNotFoundException("The password entered is incorrect");
                loginData.password = changePassword.newPassword;
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            throw new KeyNotFoundException($"The type '{changePassword.userRole}' doesn't exist");
        }

        public HttpStatusCode changeProfilePic(AddPic change)
        {
            string extension = change.file.FileName.Split(".").Last();
            extension = extension.ToLower();
            string filename;
            if (extension != "jpeg" && extension != "jpg" && extension != "png") throw new Exception("only jpeg, jpg and png files are allowed");
            if(change.role == "doctor")
            {
                var doc = context.Doctors.SingleOrDefault(doc=>doc.Id == change.id) ?? throw new KeyNotFoundException($"Doctor with ID {change.id} doesn't exist");
                filename = $"Doc{change.id}.{extension}";
                doc.PicPath = $"../../Frontend/ITIAngularproject/src/assets/profilepic/{filename}";
            }
            else if (change.role == "patient")
            {
                var pat = context.Patients.SingleOrDefault(pat => pat.Id == change.id) ?? throw new KeyNotFoundException($"Patient with ID {change.id} doesn't exist");
                filename = $"Pat{change.id}.{extension}";
                pat.PicPath = $"../../Frontend/ITIAngularproject/src/assets/profilepic/{filename}";
            }
            else
            { throw new KeyNotFoundException($"user type {change.role} doesn't exist"); }
            using (FileStream FS = new FileStream($"../../Frontend/ITIAngularproject/src/assets/profilepic/{filename}", FileMode.Create))
            {
                change.file.CopyTo(FS);
            }
            var files = new DirectoryInfo($"../../Frontend/ITIAngularproject/src/assets/profilepic").GetFiles().Where(di => di.Extension != $".{extension}" && di.Name.Split(".")[0] == filename.Split(".")[0]);
            foreach (var file in files)
            {
                File.Delete(file.FullName);
            }
            context.SaveChanges();
            return HttpStatusCode.OK;
        }
        public HttpStatusCode DeleteProfilePic(UserInfo user)
        {
            if(user.role == "doctor")
            {
                var doc = context.Doctors.SingleOrDefault(docs=>docs.Id == user.id) ?? throw new KeyNotFoundException($"Doctor with ID {user.id} doesn't exist");
                if (doc.PicPath == "../../Frontend/ITIAngularproject/src/assets/profilepic/defaultDoc.png") throw new Exception("Picture Already Deleted");
                File.Delete(doc.PicPath);
                doc.PicPath = "../../Frontend/ITIAngularproject/src/assets/profilepic/defaultDoc.png";
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            else if(user.role == "patient")
            {
                var pat = context.Patients.SingleOrDefault(pats => pats.Id == user.id) ?? throw new KeyNotFoundException($"Patient with ID {user.id} doesn't exist");
                if (pat.PicPath == null) throw new Exception("Picture Already Deleted");
                File.Delete(pat.PicPath);
                pat.PicPath = null;
                context.SaveChanges();
                return HttpStatusCode.NoContent;
            }
            throw new KeyNotFoundException($"user type {user.role} doesn't exist");

        }
    }
}