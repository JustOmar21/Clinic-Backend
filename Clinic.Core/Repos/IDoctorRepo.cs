using Clinic.Core.DTO;
using Clinic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Repos
{
    public interface IDoctorRepo
    {
        public List<SingleDoctorDetails> GetAllDoctors(int pageNumber = 1,int pageSize = 10 ,string Location = "");
        public dynamic GetDoctor(int id);
        public dynamic GetAllAppointements(int DoctorID, DateTime? date = null);
        public HttpStatusCode AddDoctor(Doctor doctor);
        public HttpStatusCode EditDoctor(Doctor doctor);
        public HttpStatusCode DeleteDoctor(int id);
        public HttpStatusCode ConfirmAppointment(int AppID);
        public HttpStatusCode CancalAppointment(int AppID);
        public HttpStatusCode RejectAppointment(int AppID);
    }
}
