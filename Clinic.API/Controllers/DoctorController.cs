using Clinic.Core.DTO;
using Clinic.Core.Models;
using Clinic.Core.Repos;
using Clinic.Infrastructure.Filters;
using Clinic.Infrastructure.RepoImplemention;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.API.Controllers
{
    [Server_NotFound_Exceptions]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepo _doctorRepo;
        public DoctorController(IDoctorRepo doctorRepo)
        {
            _doctorRepo = doctorRepo;
        }
        [HttpPost("Create")]
        public IActionResult Add(AddDoctor doc)
        {
            return StatusCode((int)_doctorRepo.AddDoctor(doc));
        }
        [HttpPut("Update")]
        public IActionResult Edit(Doctor doc)
        {
            return StatusCode((int)_doctorRepo.EditDoctor(doc));
        }
        [HttpDelete("Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            return StatusCode((int)_doctorRepo.DeleteDoctor(id));
        }
        [HttpGet("{id:int}")]
        public IActionResult GetSingleDoctor(int id)
        {
            var patient = _doctorRepo.GetDoctor(id);
            return Ok(patient);
        }
        [HttpGet("Details/{id:int}")]
        public IActionResult GetSingleDoctorDetails(int id)
        {
            var patient = _doctorRepo.GetDoctorDetails(id);
            return Ok(patient);
        }
        [HttpGet("page={pageNumber:int}&size={pageSize:int}&Location={Location:alpha}&email={email:alpha}&specialityID={specialityID:int}")]
        [HttpGet("page={pageNumber:int}&size={pageSize:int}&Location={Location:alpha}&email={email:alpha}")]
        [HttpGet("page={pageNumber:int}&size={pageSize:int}&Location={Location:alpha}&specialityID={specialityID:int}")]
        [HttpGet("page={pageNumber:int}&size={pageSize:int}&email={email:alpha}&specialityID={specialityID:int}")]
        [HttpGet("page={pageNumber:int}&size={pageSize:int}&Location={Location:alpha}")]
        [HttpGet("page={pageNumber:int}&size={pageSize:int}&email={email:alpha}")]
        [HttpGet("page={pageNumber:int}&size={pageSize:int}&specialityID={specialityID:int}")]
        [HttpGet("page={pageNumber:int}&size={pageSize:int}")]
        public IActionResult GetAllDoctors(int pageNumber, int pageSize, string Location = "", int specialityID = -1, string email = "")
        {
            var patient = _doctorRepo.GetAllDoctors(pageNumber, pageSize, Location, specialityID , email);
            return Ok(patient);
        }
        [HttpGet("Details/page={pageNumber:int}&size={pageSize:int}&Location={Location:alpha}&email={email:alpha}&specialityID={specialityID:int}")]
        [HttpGet("Details/page={pageNumber:int}&size={pageSize:int}&Location={Location:alpha}&email={email:alpha}")]
        [HttpGet("Details/page={pageNumber:int}&size={pageSize:int}&Location={Location:alpha}&specialityID={specialityID:int}")]
        [HttpGet("Details/page={pageNumber:int}&size={pageSize:int}&email={email:alpha}&specialityID={specialityID:int}")]
        [HttpGet("Details/page={pageNumber:int}&size={pageSize:int}&Location={Location:alpha}")]
        [HttpGet("Details/page={pageNumber:int}&size={pageSize:int}&email={email:alpha}")]
        [HttpGet("Details/page={pageNumber:int}&size={pageSize:int}&specialityID={specialityID:int}")]
        [HttpGet("Details/page={pageNumber:int}&size={pageSize:int}")]
        public IActionResult GetAllDoctorsDetails(int pageNumber, int pageSize, string Location = "", int specialityID = -1, string email = "")
        {
            var patient = _doctorRepo.GetAllDoctorsDetails(pageNumber, pageSize, Location, specialityID, email);
            return Ok(patient);
        }
        [HttpGet("Count/&Location={Location:alpha}&email={email:alpha}&specialityID={specialityID:int}")]
        [HttpGet("Count/&Location={Location:alpha}&email={email:alpha}")]
        [HttpGet("Count/&Location={Location:alpha}&specialityID={specialityID:int}")]
        [HttpGet("Count/&email={email:alpha}&specialityID={specialityID:int}")]
        [HttpGet("Count/&Location={Location:alpha}")]
        [HttpGet("Count/&email={email:alpha}")]
        [HttpGet("Count/&specialityID={specialityID:int}")]
        [HttpGet("Count")]
        public IActionResult GetAllDoctorsCount(int pageNumber, int pageSize, string Location = "", int specialityID = -1, string email = "")
        {
            var patient = _doctorRepo.GetDoctorsCount(Location, specialityID, email);
            return Ok(patient);
        }
        [HttpGet("Appointment/doctor={doctorID:int}&pagenumber={pageNumber:int}&pagesize={pageSize:int}")]
        [HttpGet("Appointment/doctor={doctorID:int}&date={date:datetime}&pagenumber={pageNumber:int}&pagesize={pageSize:int}")]
        public IActionResult GetAllAppointments(int doctorID, DateTime? date, int pageNumber = 1, int pageSize = 10)
        {
            if (date is not null)
                date = date.Value.Date;
            return Ok(_doctorRepo.GetAllAppointements(doctorID, date, pageNumber, pageSize));
        }
        [HttpGet("Appointment/Count/doctor={doctorID:int}&pagenumber={pageNumber:int}&pagesize={pageSize:int}")]
        [HttpGet("Appointment/Count/doctor={doctorID:int}&date={date:datetime}&pagenumber={pageNumber:int}&pagesize={pageSize:int}")]
        public IActionResult GetAllAppointmentsCount(int doctorID, DateTime? date)
        {
            if (date is not null)
                date = date.Value.Date;
            return Ok(_doctorRepo.GetAllAppointementsCount(doctorID, date));
        }
        [HttpGet("Appointment/doctor={docID:int}&patient={patientID:int}&pagenumber={pageNumber:int}&pagesize={pageSize:int}")]
        public IActionResult GetDocAppointments(int docID, int patientID, int pageNumber = 1, int pageSize = 10)
        {
            return Ok(_doctorRepo.GetPatientAppointements(docID, patientID, pageNumber, pageSize));
        }
        [HttpGet("Appointment/Count/doctor={docID:int}&patient={patientID:int}&pagenumber={pageNumber:int}&pagesize={pageSize:int}")]
        public IActionResult GetDocAppointmentsCount(int docID, int patientID, int pageNumber = 1, int pageSize = 10)
        {
            return Ok(_doctorRepo.GetPatientAppointementsCount(docID, patientID));
        }
        [HttpPatch("Appointment/Cancel/{appID:int}")]
        public IActionResult CancelAppointment(int appID)
        {
            return StatusCode((int)_doctorRepo.CancelAppointment(appID));
        }
        [HttpPatch("Appointment/Reject/{appID:int}")]
        public IActionResult RejectAppointment(int appID)
        {
            return StatusCode((int)_doctorRepo.RejectAppointment(appID));
        }
        [HttpPatch("Appointment/Confirm/{appID:int}")]
        public IActionResult ConfirmAppointment(int appID)
        {
            return StatusCode((int)_doctorRepo.ConfirmAppointment(appID));
        }
        [HttpGet("Schedule/{docID:int}")]
        public IActionResult GetSchedule(int docID)
        {
            return Ok(_doctorRepo.GetSchedule(docID));
        }
        [HttpPost("Schedule/Add")]
        public IActionResult AddSchedule(Schedule schedule)
        {
            return StatusCode((int)_doctorRepo.AddSchedule(schedule));
        }
        [HttpPut("Schedule/Edit")]
        public IActionResult EditSchedule(Schedule schedule)
        {
            return StatusCode((int)_doctorRepo.EditSchedule(schedule));
        }
        [HttpGet("Speciality/{specialityID:int}")]
        public IActionResult GetSpeciality(int specialityID)
        {
            return Ok(_doctorRepo.GetSpeciality(specialityID));
        }
        [HttpDelete("Speciality/Delete/{specialityID:int}")]
        public IActionResult DeleteSpeciality(int specialityID)
        {
            return Ok(_doctorRepo.DeleteSpeciality(specialityID));
        }
        [HttpPost("Speciality/Add")]
        public IActionResult AddSpeciality(Speciality speciality)
        {
            return StatusCode((int)_doctorRepo.AddSpeciality(speciality));
        }
        [HttpPut("Speciality/Edit")]
        public IActionResult EditSpeciality(Speciality speciality)
        {
            return StatusCode((int)_doctorRepo.EditSpeciality(speciality));
        }
        [HttpGet("Speciality")]
        public IActionResult GetAllSpeciality()
        {
            return Ok(_doctorRepo.GetAllSpeciality());
        }

        [HttpPost("Appointment/Order")]
        public IActionResult GetCurrentOrder(DateTime date)
        {
            return Ok(_doctorRepo.GetCurrentOrder(date));
        }
        [HttpGet("Details")]
        public IActionResult GetDocs()
        {
            return Ok(_doctorRepo.GetDoctorsWithSpec());
        }

        [HttpGet("Appointment/Requested/doctor={doctorID:int}")]
        [HttpGet("Appointment/Requested/doctor={doctorID:int}&date={date:datetime}")]
        public IActionResult GetAllRequestedAppointments(int doctorID, DateTime? date)
        {
            if (date is not null) date = date.Value.Date;
            return Ok(_doctorRepo.GetAllRequestedAppointments(doctorID, date));
        }
        [HttpGet("Appointment/Others/doctor={doctorID:int}")]
        [HttpGet("Appointment/Others/doctor={doctorID:int}&date={date:datetime}")]
        public IActionResult GetAllOtherAppointments(int doctorID, DateTime? date)
        {
            if (date is not null) date = date.Value.Date;
            return Ok(_doctorRepo.GetAllOtherAppointments(doctorID, date));
        }
        [HttpGet("Appointment/Accepted/doctor={doctorID:int}")]
        [HttpGet("Appointment/Accepted/doctor={doctorID:int}&date={date:datetime}")]
        public IActionResult GetAllAcceptedAppointments(int doctorID, DateTime? date)
        {
            if (date is not null) date = date.Value.Date;
            return Ok(_doctorRepo.GetAllAcceptedAppointments(doctorID, date));
        }
        [HttpPost("Document/Add")]
        public IActionResult AddDocument([FromForm]AddDocument document)
        {
            return StatusCode((int)_doctorRepo.AddDocument(document));
        }
        [HttpDelete("Document/Delete/{documentID:int}")]
        public IActionResult DeleteDocument(int documentID)
        {
            return StatusCode((int)_doctorRepo.DeleteDocument(documentID));
        }
        [HttpGet("/Document/nid&cert/{docID:int}")]
        public IActionResult GetNidCert(int docID)
        {
            return Ok(_doctorRepo.GetNIDCerts(docID));
        }
    }
}
