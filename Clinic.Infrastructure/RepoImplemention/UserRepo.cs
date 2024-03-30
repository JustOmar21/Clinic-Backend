using Clinic.Core.DTO;
using Clinic.Core.Models;
using Clinic.Core.Repos;
using Clinic.Infrastructure.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
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
    }
}