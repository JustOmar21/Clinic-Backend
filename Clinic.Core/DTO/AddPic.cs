using Clinic.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.DTO
{
    public class AddPic
    {
        public int id { get; set; }
        public string role {  get; set; }
        public IFormFile file { get; set; }
    }
}
