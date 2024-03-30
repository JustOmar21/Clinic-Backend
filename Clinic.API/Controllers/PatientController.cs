using Clinic.Core.DTO;
using Clinic.Core.Models;
using Clinic.Core.Repos;
using Clinic.Infrastructure.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Clinic.API.Controllers
{
    [Server_NotFound_Exceptions]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepo _patientRepo;
        public PatientController(IPatientRepo patientRepo)
        {
            _patientRepo = patientRepo;
        }
        [HttpPost("Create")]
        public IActionResult Add(AddPatient pat)
        {
            return StatusCode((int)_patientRepo.AddPatient(pat));
        }
        [HttpPut("Update")]
        public IActionResult Edit(Patient pat)
        {
           return StatusCode((int)_patientRepo.EditPatient(pat));
        }
        [HttpDelete("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
             return StatusCode((int)_patientRepo.DeletePatient(id));
        }
        [HttpGet("{id:int}")]
        public IActionResult GetSinglePatient(int id)
        {
            var patient = _patientRepo.GetPatient(id);
            return Ok(patient);
        }
        [HttpGet("page={pageNumber:int}&size={pageSize:int}&name={name:alpha}&email={email:alpha}")]
        [HttpGet("page={pageNumber:int}&size={pageSize:int}&name={name:alpha}")]
        [HttpGet("page={pageNumber:int}&size={pageSize:int}&email={email:alpha}")]
        [HttpGet("page={pageNumber:int}&size={pageSize:int}")]
        public IActionResult GetAllPatient(int pageNumber, int pageSize, string name = "", string email = "")
        {
            var patient = _patientRepo.GetAllPatient(pageNumber,pageSize,name,email);
            return Ok(patient);
        }
        [HttpPost("Appointment/Add")]
        public IActionResult AddAppointment(Appointement appointement)
        {
            appointement.CreatedDate = appointement.CreatedDate.Date;
            appointement.Date = appointement.Date.Date;
            return StatusCode((int)_patientRepo.AddAppointment(appointement));
        }
        [HttpPatch("Appointment/Cancel/{appID:int}")]
        public IActionResult CancelAppointment(int appID)
        {
            return StatusCode((int)_patientRepo.CancelAppointment(appID));
        }
        [HttpPost("Card/Add")]
        public IActionResult AddCard(Paycard card)
        {
            return StatusCode((int)_patientRepo.AddCard(card));
        }
        [HttpPut("Card/Update")]
        public IActionResult EditCard([FromBody]Paycard card)
        {
            return StatusCode((int)_patientRepo.EditCard(card));
        }
        [HttpDelete("Card/Delete/{cardID:int}")]
        public IActionResult DeleteCard(int cardID)
        {
            return StatusCode((int)_patientRepo.DeleteCard(cardID));
        }

        [HttpGet("Card/{cardID:int}")]
        public IActionResult GetPatientCard(int cardID)
        {
            return Ok(_patientRepo.GetCard(cardID));
        }
        [HttpGet("Count")]
        [HttpGet("Count/name={name}")]
        [HttpGet("Count/email={email}")]
        [HttpGet("Count/name={name}&email={email}")]
        public IActionResult GetPatientCount(string name = "", string email = "")
        {
            return Ok(_patientRepo.GetPatientCount(name,email));
        }

        [HttpGet("Appointment/patient={patientID:int}&pagenumber={pageNumber:int}&pagesize={pageSize:int}")]
        [HttpGet("Appointment/patient={patientID:int}&date={date:datetime}&pagenumber={pageNumber:int}&pagesize={pageSize:int}")]
        public IActionResult GetAllAppointments(int patientID,DateTime? date,int pageNumber = 1, int pageSize = 10)
        {
            if(date is not null)
            date = date.Value.Date;
            return Ok(_patientRepo.GetAllAppointements(patientID,date,pageNumber,pageSize));
        }
        [HttpGet("Appointment/Count/patient={patientID:int}&pagenumber={pageNumber:int}&pagesize={pageSize:int}")]
        [HttpGet("Appointment/Count/patient={patientID:int}&date={date:datetime}&pagenumber={pageNumber:int}&pagesize={pageSize:int}")]
        public IActionResult GetAllAppointmentsCount(int patientID, DateTime? date)
        {
            if (date is not null)
                date = date.Value.Date;
            return Ok(_patientRepo.GetAllAppointementsCount(patientID, date));
        }
        [HttpGet("Appointment/doctor={docID:int}&patient={patientID:int}&pagenumber={pageNumber:int}&pagesize={pageSize:int}")]
        public IActionResult GetDocAppointments(int docID, int patientID, int pageNumber = 1, int pageSize = 10)
        {
            return Ok(_patientRepo.GetAppointements(docID, patientID,pageNumber,pageSize));
        }
        [HttpGet("Appointment/Count/doctor={docID:int}&patient={patientID:int}&pagenumber={pageNumber:int}&pagesize={pageSize:int}")]
        public IActionResult GetDocAppointmentsCount(int docID, int patientID, int pageNumber = 1, int pageSize = 10)
        {
            return Ok(_patientRepo.GetAppointementsCount(docID, patientID));
        }

        [HttpGet("Review/{reviewID:int}")]
        public IActionResult GetReview(int reviewID)
        {
            return Ok(_patientRepo.GetReview(reviewID));
        }
        [HttpPost("Review/Add")]
        public IActionResult AddReview(Review review)
        {
            return StatusCode((int)_patientRepo.AddReview(review));
        }
        [HttpPut("Review/Edit")]
        public IActionResult EditReview(Review review)
        {
            return StatusCode((int)_patientRepo.EditReview(review));
        }
        [HttpGet("Details")]
        public IActionResult GetAllPatientsNoPage()
        {
            return Ok(_patientRepo.GetAllPatientNoPage());
        }
        [HttpGet("AppointmentDocs/{patientID:int}")]
        public IActionResult GetAppsWithDoc(int patientID)
        {
            return Ok(_patientRepo.GetAllApps(patientID));
        }
    }
}
