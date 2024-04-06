using Clinic.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.DTO
{
    public class AccountActiveDocs
    {
        public Documents? nid { get; set; }
        public List<Documents>? Certificates { get; set; }
    }
}
