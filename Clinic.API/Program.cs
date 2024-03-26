using Clinic.Core.Repos;
using Clinic.Infrastructure.DBContext;
using Clinic.Infrastructure.RepoImplemention;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;

namespace Clinic.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            ///////////////////////////////
            //builder.Services.Configure<JsonOptions>(options =>
            //{
            //    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            //    options.JsonSerializerOptions.WriteIndented = true;
            //});
            builder.Services.AddTransient<IDoctorRepo, DoctorRepo>();
            builder.Services.AddTransient<IPatientRepo, PatientRepo>();
            builder.Services.AddTransient<IAdminRepo, AdminRepo>();
            builder.Services.AddDbContext<ClinicDBContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("connect"));
            });


            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ClinicDBContext>()
            .AddDefaultTokenProviders();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
