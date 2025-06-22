using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vaccine.API.Models.FeedbackModel;
using Vaccine.Repo.Entities;
using Vaccine.Repo.UnitOfWork;

namespace Vaccine.API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class FeedBackController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public FeedBackController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("create-feedback")]
        public async Task<IActionResult> CreateHoliday([FromBody] RequestCreateFeedBackModel newFeedBack)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra xem CustomerId có đúng không
            var customer = _unitOfWork.CustomerRepository
                .GetQueryable().Where(x => x.CustomerId == newFeedBack.CustomerId)
                .FirstOrDefault();
            // Kiểm tra xem DoctorId có đúng không
            var doctor = _unitOfWork.DoctorRepository
               .GetQueryable().Where(x => x.DoctorId == newFeedBack.DoctorId)
               .FirstOrDefault();
            // Kiểm tra xem StaffId có đúng không
            var staff = _unitOfWork.StaffRepository
               .GetQueryable().Where(x => x.StaffId == newFeedBack.StaffId)
               .FirstOrDefault();
            // Kiểm tra xem VaccineId có đúng không
            var vaccine = _unitOfWork.VaccineRepository
               .GetQueryable().Where(x => x.VaccineId == newFeedBack.VaccineId)
               .FirstOrDefault();
            // Kiểm tra xem AppointmentId có đúng không
            var appointment = _unitOfWork.AppointmentRepository
               .GetQueryable().Where(x => x.AppointmentId == newFeedBack.AppointmentId)
               .FirstOrDefault();

            if (customer == null)
            {
                return BadRequest("Customer not found");
            }

            if (doctor == null)
            {
                return BadRequest("Doctor not found");
            }

            if (staff == null)
            {
                return BadRequest("Staff not found");
            }

            if (vaccine == null)
            {
                return BadRequest("Vaccine not found");
            }

            if (appointment == null)
            {
                return BadRequest("Appointment not found");
            }

            var feedBack = new Feedback
            {
                CustomerId = newFeedBack.CustomerId,
                DoctorId = newFeedBack.DoctorId,
                StaffId = newFeedBack.StaffId,
                VaccineId = newFeedBack.VaccineId,
                AppointmentId = newFeedBack.AppointmentId,
                Rating = newFeedBack.Rating,
                Comment = newFeedBack.Comment,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            _unitOfWork.FeedbackRepository.Insert(feedBack);
            await _unitOfWork.SaveAsync();

            return Ok(new { Message = "FeedBack created successfully", feedBack });
        }

        [HttpGet("get-feedback-by-id/{id}")]
        public IActionResult GetFeedbackById(int id)
        {
            // 
            var feedback = _unitOfWork.FeedbackRepository.GetQueryable().Include(x => x.Customer)
                                                                        .Include(x => x.Appointment)
                                                                        .Include(x => x.Doctor)
                                                                        .Include(x => x.Staff)
                                                                        .Where(x => x.ReviewId == id)
                                                                        .FirstOrDefault();

            if (feedback == null)
            {
                return NotFound(new { message = "Feedback not found" });
            }

            var feedbackViewModel = new FeedbackViewModel(feedback);

            return Ok(feedbackViewModel);

        }

        [HttpGet("get-feedback")]
        public IActionResult GetFeedbacks()
        {
            var feedback = _unitOfWork.FeedbackRepository.GetQueryable().Include(x => x.Customer)
                                                                       .Include(x => x.Appointment)
                                                                       .Include(x => x.Doctor)
                                                                       .Include(x => x.Staff)
                                                                       .Include(x => x.Vaccine)
                                                                       .ToList();

            if (feedback.Count == 0)
            {
                return Ok();
            }

            var result = feedback.Select(x => new FeedbackViewModel(x));
            return Ok(result);

        }
        [HttpGet("get-success-appointments-pending-feedback/{customerId}")]
        public IActionResult GetSuccessAppointmentsPendingFeedback(int customerId)
        {
            // Lấy danh sách cuộc hẹn có trạng thái "Success" của khách hàng
            var successAppointments = _unitOfWork.AppointmentRepository.GetQueryable()
                .Where(a => a.CustomerId == customerId && a.Status == "Success")
                .Select(a => new
                {
                    a.AppointmentId,
                    a.AppointmentDate,
                    a.VaccineId,
                    VaccineName = a.Vaccine.Name,
                    VaccineType = a.VaccineType,
                    DoctorName = a.Doctor != null ? a.Doctor.Name : "N/A"
                })
                .ToList();

            // Lấy danh sách các cuộc hẹn đã có feedback của khách hàng
            var appointmentsWithFeedback = _unitOfWork.FeedbackRepository.GetQueryable()
                .Where(f => f.CustomerId == customerId)
                .Select(f => f.AppointmentId)
                .ToList();

            // Lọc ra các cuộc hẹn chưa có feedback
            var pendingAppointments = successAppointments
                .Where(a => !appointmentsWithFeedback.Contains(a.AppointmentId))
                .ToList();

            if (!pendingAppointments.Any())
            {
                return Ok(new { message = "There is no response from this appointment." });
            }

            return Ok(pendingAppointments);
        }
    }
}
