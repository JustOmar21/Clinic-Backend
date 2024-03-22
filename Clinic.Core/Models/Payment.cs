using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string ExpirationDate { get; set; }
        public string CCV { get; set; }

    }
}
