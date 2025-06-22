using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.API.Models.CustomerModel;
using Swashbuckle.AspNetCore.Annotations;
using Vaccine.API.Models;
using Vaccine.API.Models.AppointmentModel;
using Vaccine.API.Models.CustomerModel;
using Vaccine.API.Models.HealthRecordsModel;
using Vaccine.API.Models.InvoiceModel;
using Vaccine.Repo.Entities;
using Vaccine.Repo.UnitOfWork;

namespace Vaccine.API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class HealthRecordController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public HealthRecordController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("get-health-records")]
        public IActionResult GetHealthRecords()
        {
            var healths = _unitOfWork.HealthRecordRepository.Get();
            return Ok(healths);
        }

        [HttpGet("get-health-records/{recordId}")]
        public IActionResult GetHealthRecordsByID(int recordId)
        {
            //var health = _unitOfWork.HealthRecordRepository.GetByID(recordId);
            var health = _unitOfWork.HealthRecordRepository
        .Get(filter: h => h.RecordId == recordId, includeProperties: "Appointment")
        .FirstOrDefault();

            if (health == null)
            {
                return NotFound(new { message = "Health records not found." });
            }
            //return Ok(health);
            return Ok(new 
            {
                RecordId = health.RecordId,
                StaffId = health.StaffId,
                AppointmentId = health.AppointmentId,
                DoctorId = health.DoctorId,
                BloodPressure = health.BloodPressure,
                HeartRate = health.HeartRate,
                Height = health.Height,
                Weight = health.Weight,
                Temperature = health.Temperature,
                AteBeforeVaccine = health.AteBeforeVaccine,
                ConditionOk = health.ConditionOk,
                ReactionNotes = health.ReactionNotes,
                CreatedAt = health.CreatedAt,
                // get appointment by appoint_id
                Appointment = health.Appointment != null ? new 
                {
                    AppointmentId = health.Appointment.AppointmentId,
                    AppointmentDate = health.Appointment.AppointmentDate,
                    Status = health.Appointment.Status
                } : null
            });
        }

        [HttpPost("create-health-record")]
        [SwaggerOperation(Description = "Creates a health record. Requires appointment_id.")]
        public IActionResult CreateHealthRecord([FromBody] RequestCreateHealthRecordsModel request)
        {
            // Retrieve the appointment
            var appointment = _unitOfWork.AppointmentRepository.GetByID(request.AppointmentId);

            if (request == null || request.AppointmentId == null)
            {
                return BadRequest(new { message = "Appointment ID is required." });
            }
            // para input to create 
            var healthEntity = new HealthRecord
            {
                StaffId = request.StaffId == 0 ? null : request.StaffId,
                AppointmentId = request.AppointmentId ,
                DoctorId = request.DoctorId == 0 ? null : request.StaffId,
                BloodPressure = request.BloodPressure,
                HeartRate = request.HeartRate,
                Height = request.Height,
                Weight = request.Weight,
                Temperature = request.Temperature,
                AteBeforeVaccine = request.AteBeforeVaccine,
                ConditionOk = request.ConditionOk,
                ReactionNotes = request.ReactionNotes,
                CreatedAt = request.CreatedAt ?? DateTime.UtcNow,
            };

            _unitOfWork.HealthRecordRepository.Insert(healthEntity);

            // Update appointment status to "Success"
            appointment.Status = "Success";
            _unitOfWork.AppointmentRepository.Update(appointment);

            _unitOfWork.Save();
            return Ok(healthEntity);
        }

        

        [HttpDelete("delete-appointment/{id}")]

        public IActionResult DeleteAppointment(int id)
        {
            var appointment = _unitOfWork.AppointmentRepository.GetByID(id);
            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found." });
            }
            _unitOfWork.AppointmentRepository.Delete(appointment);
            _unitOfWork.Save();
            return Ok(new { message = "Appointment deleted successfully." });
        }


        [HttpPut("update-appointment-date/{id}")]
        public async Task<IActionResult> UpdateAppointmentDate(int id, [FromBody] DateTime newDate)
        {
            var appointment = _unitOfWork.AppointmentRepository.GetByID(id);
            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found." });
            }

            // Ensure the new date is not before the existing date
            if (newDate < appointment.AppointmentDate)
            {
                return BadRequest(new { message = "Cannot update to a past date." });
            }

            // Update only the AppointmentDate field
            appointment.AppointmentDate = newDate;
            _unitOfWork.Save();

            return Ok(new { message = "Appointment date updated successfully." });
        }

    }
}
