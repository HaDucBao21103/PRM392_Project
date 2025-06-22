using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vaccine.API.Models.HolidayModel;
using Vaccine.Repo.Entities;
using Vaccine.Repo.UnitOfWork;

namespace Vaccine.API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public HolidayController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("create-holiday")]
        public async Task<IActionResult> CreateHoliday([FromBody] RequestCreateHolidayModel newHoliday)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra xem adminId có đúng không
            var admin = _unitOfWork.AdminRepository
                .GetQueryable().Where(x => x.AdminId == newHoliday.AdminId)
                .FirstOrDefault();

            if (admin == null)
            {
                return BadRequest("Admin not found");
            }

            //check ngày bắt đầu không được lớn hơn ngày kết thúc
            if (newHoliday.DateFrom > newHoliday.DateTo)
            {
                return BadRequest("DateFrom is greater then DateTo");
            }

            var holiday = new Holiday
            {
                AdminId = newHoliday.AdminId,
                DateFrom = newHoliday.DateFrom,
                DateTo = newHoliday.DateTo,
                Reason = newHoliday.Reason,
            };

            _unitOfWork.HolidayRepository.Insert(holiday);
            await _unitOfWork.SaveAsync();

            return Ok(new { Message = "Holiday created successfully", holiday });
        }

        [HttpPut("update-holiday/{id}")]
        public IActionResult UpdateHoliday(int id, RequestCreateHolidayModel updateHoliday)
        {
            if (updateHoliday == null)
            {
                return BadRequest(new { message = "Holiday data is required" });
            }

            // Kiểm tra xem adminId có đúng không
            var admin = _unitOfWork.AdminRepository
                .GetQueryable().Where(x => x.AdminId == updateHoliday.AdminId)
                .FirstOrDefault();

            if (admin == null)
            {
                return BadRequest("Admin not found");
            }

            //check ngày bắt đầu không được lớn hơn ngày kết thúc
            if (updateHoliday.DateFrom > updateHoliday.DateTo)
            {
                return BadRequest("DateFrom is greater then DateTo");
            }
            // Tìm holiday theo ID
            var holiday = _unitOfWork.HolidayRepository.GetByID(id);
            if (holiday == null)
            {
                return NotFound(new { message = "Holiday not found" });
            }

            // Cập nhật thông tin holiday
            holiday.AdminId = updateHoliday.AdminId;
            holiday.DateFrom = updateHoliday.DateFrom;
            holiday.DateTo = updateHoliday.DateTo;
            holiday.Reason = updateHoliday.Reason;

            // Lưu thay đổi vào database
            _unitOfWork.HolidayRepository.Update(holiday);
            _unitOfWork.Save();

            return Ok(holiday);
        }

        [HttpDelete("delete-holiday/{id}")]
        public IActionResult DeleteHoliday(int id)
        {
            // Tìm holiday theo ID
            var holiday = _unitOfWork.HolidayRepository.GetByID(id);
            if (holiday == null)
            {
                return NotFound(new { message = "Holiday not found" });
            }

            // Lưu thay đổi vào database
            _unitOfWork.HolidayRepository.Delete(holiday);
            _unitOfWork.Save();

            return Ok("Delete Success");
        }

        [HttpGet("get-holiday-by-id/{id}")]
        public IActionResult GetHolidayById(int id) {
            // 
            var holiday = _unitOfWork.HolidayRepository.GetQueryable().Include(x => x.Admin).Where(x => x.HolidayId == id).Select(x => new
            {
                x.HolidayId,
                x.Admin.Name,
                x.Admin.AdminId,
                x.DateFrom, 
                x.DateTo,
                x.Reason
            }).FirstOrDefault();

            if (holiday == null)
            {
                return NotFound(new { message = "Holiday not found" });
            }
            return Ok(holiday);
        }

        [HttpGet("get-holidays")]
        public IActionResult GetHolidays()
        {
            var holidays = _unitOfWork.HolidayRepository.GetQueryable().Include(x => x.Admin).Select(x => new
            {
                x.HolidayId,
                x.Admin.Name,
                x.Admin.AdminId,
                x.DateFrom,
                x.DateTo,
                x.Reason
            }).ToList();

            if (holidays.Count == 0)
            {
                return Ok();
            }
            return Ok(holidays);
        }
    }
}
