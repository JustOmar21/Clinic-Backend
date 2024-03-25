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
        public HttpStatusCode ActivateAccount(int accountID, string accountType);
        public HttpStatusCode DeactivateAccount(int accountID, string accountType);
        public HttpStatusCode BanAccount(int accountID, string accountType);
        public HttpStatusCode ConfirmationRequest(int accountID, string accountType);
        public dynamic GetKeypass(int accountID, string accountType);
    }
}
