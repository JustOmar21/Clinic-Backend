using Clinic.Core.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Repos
{
    public interface IUserRepo
    {
        public UserInfo Login(LoginDTO log);
        public HttpStatusCode changePassword(ChangePassword changePassword);

        public HttpStatusCode changeProfilePic(AddPic change);

        public HttpStatusCode DeleteProfilePic(UserInfo user);
    }
}
