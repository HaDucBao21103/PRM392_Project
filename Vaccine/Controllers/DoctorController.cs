using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Vaccine.API.Models.DoctorModel;
using Vaccine.Repo.Entities;
using Vaccine.Repo.UnitOfWork;

namespace Vaccine.API.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        public DoctorController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("create-doctor")]
        public async Task<IActionResult> CreateDoctor([FromBody] RequestCreateDoctorModel newDoctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra số điện thoại đã tồn tại chưa //
            var existingDoctor = _unitOfWork.DoctorRepository
                .Get(d => d.Phone == newDoctor.Phone)
                .FirstOrDefault();

            if (existingDoctor != null)
            {
                return BadRequest("Doctor with this phone number already exists.");
            }

            var doctor = new Doctor
            {
                Name = newDoctor.Name,
                Age = newDoctor.Age,
                Gender = newDoctor.Gender,
                Phone = newDoctor.Phone,
                Address = newDoctor.Address,
                Degree = newDoctor.Degree,
                ExperienceYears = newDoctor.ExperienceYears,
                Dob = newDoctor.Dob
            };

            _unitOfWork.DoctorRepository.Insert(doctor);
            await _unitOfWork.SaveAsync();

            return Ok(new { Message = "Doctor created successfully", doctor });
        }

        [HttpPut("update-doctor/{doctorId}")]
        public async Task<IActionResult> UpdateDoctor(int doctorId, [FromBody] RequestCreateDoctorModel updatedDoctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var doctor = _unitOfWork.DoctorRepository.GetByID(doctorId);

            if (doctor == null)
            {
                return NotFound("Doctor not found.");
            }

            // Cập nhật thông tin
            doctor.Name = updatedDoctor.Name;
            doctor.Age = updatedDoctor.Age;
            doctor.Gender = updatedDoctor.Gender;
            doctor.Phone = updatedDoctor.Phone;
            doctor.Address = updatedDoctor.Address;
            doctor.Degree = updatedDoctor.Degree;
            doctor.ExperienceYears = updatedDoctor.ExperienceYears;
            doctor.Dob = updatedDoctor.Dob;

            _unitOfWork.DoctorRepository.Update(doctor);
            await _unitOfWork.SaveAsync();

            return Ok(new { Message = "Doctor updated successfully", doctor });
        }

        [HttpGet("get-doctor-by-id/{id}")]
        public IActionResult GetDoctorById(int id)
        {
            {
                var doctor = _unitOfWork.DoctorRepository.GetQueryable().Where(x => x.DoctorId == id).Select(x => new
                {
                    x.DoctorId,
                    x.Name,
                    x.Age,
                    x.Gender,
                    x.Phone,
                    x.Address,
                    x.Degree,
                    x.ExperienceYears,
                }).FirstOrDefault();

                if (doctor == null)
                {
                    return NotFound(new { message = "Doctor not found" });
                }

                return Ok(doctor);

            }
        }

        [HttpGet("get-doctors")]
        public IActionResult GetDoctors()
        {
            var doctors = _unitOfWork.DoctorRepository.GetQueryable().Select(x => new
            {
                x.DoctorId,
                x.Name,
                x.Age,
                x.Gender,
                x.Phone,
                x.Address,
                x.Degree,
                x.ExperienceYears,
            }).ToList();

            if (doctors.Count == 0)
            {
                return Ok();
            }

            return Ok(doctors);

        }

    }

}
