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
    public class ClinicDBContext : DbContext
    {
        public ClinicDBContext(DbContextOptions<ClinicDBContext> options) : base(options) { }
        DbSet<Doctor> Doctors { get; set; }
        DbSet<Patient> Patients { get; set; }
        DbSet<Appointement> Appointements { get; set; }
        DbSet<Speciality> Speciality { get; set; }
        DbSet<Schedule> Schedule { get; set; }
        DbSet<Review> Reviews { get; set; }
        DbSet<Paycard> Paycard { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Doctor>().HasIndex(doc => doc.Email).IsUnique();
            modelBuilder.Entity<Patient>().HasIndex(pat => pat.Email).IsUnique();
            modelBuilder.Entity<Appointement>().HasIndex(app => new { app.Date, app.Order });
        }
    }
}
