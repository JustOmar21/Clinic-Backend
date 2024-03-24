using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Core.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Gender { get; set; }
        public DateTime DOB { get; set; }
        public int Status { get; set; }
        public int? PaycardID { get; set; }
        public virtual Paycard? Paycard { get; set; }
        public virtual IEnumerable<Review>? Reviews { get; set; }
        public virtual IEnumerable<Appointement>? Appointements { get; set; }
    }
}
