﻿using Clinic.Core.Models;
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
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointement> Appointements { get; set; }
        public DbSet<Speciality> Speciality { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Paycard> Paycard { get; set; }
        public DbSet<confirmEmail> ConfirmEmail { get; set; }
        public DbSet<Documents> Documents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Doctor>().HasIndex(doc => doc.Email).IsUnique();
            modelBuilder.Entity<Patient>().HasIndex(pat => pat.Email).IsUnique();
            modelBuilder.Entity<Appointement>().HasIndex(app => new { app.Date, app.Order }).IsUnique();
            modelBuilder.Entity<Appointement>().HasIndex(app => new { app.PatientID, app.DoctorID, app.Date }).IsUnique();
            modelBuilder.Entity<Paycard>().HasIndex(app => app.PatientID).IsUnique();
            modelBuilder.Entity<Schedule>().HasIndex(scd => scd.DoctorID).IsUnique();
            modelBuilder.Entity<Review>().HasIndex(rev=> new {rev.DoctorID, rev.PatientID, rev.date}).IsUnique();
            modelBuilder.Entity<Login>().HasIndex(log=>log.username).IsUnique();
            modelBuilder.Entity<Login>().HasData(
                new Login() { Id = 1001 , username="admin@gmail.com" , password="123456789sS" , type = "admin" }
            );
        }
    }
}
