using Clinic.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Repos
{
    public interface IAdminRepo
    {
        public HttpStatusCode ActivateAccount(AccountStatus account);
        public HttpStatusCode DeactivateAccount(AccountStatus account);
        public HttpStatusCode BanAccount(AccountStatus account);
        public HttpStatusCode ConfirmationRequest(AccountStatus account);
        public string GetKeypass(AccountStatus account);
    }
}
