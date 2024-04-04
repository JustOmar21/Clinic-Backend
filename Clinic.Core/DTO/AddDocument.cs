using Clinic.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.DTO
{
    public class AddDocument
    {
        public int DoctorID { get; set; }
        public DocumentType DocType { get; set; }
        public IFormFile Document { get; set; }
    }
}
