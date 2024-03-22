using Clinic.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Infrastructure.DBContext
{
    public class ClinicDBContext : IdentityDbContext<IdentityUser>
    {
        public ClinicDBContext(DbContextOptions<ClinicDBContext> options) : base(options) { }
        DbSet<Doctor> Doctors { get; set; }
        DbSet<Patient> Patients { get; set; }
        DbSet<Appointement> Appointements { get; set; }
        DbSet<Speciality> Speciality { get; set; }
        DbSet<Avaliable> Avaliable { get; set; }
        DbSet<Review> Reviews { get; set; }
        DbSet<Payment> Payment { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
