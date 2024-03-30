using Clinic.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Repos
{
    public interface IUserRepo
    {
        public UserInfo Login(LoginDTO log);
    }
}
