using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.PortableExecutable;
using Vaccine.Repo.Entities;
using Vaccine.Repo.UnitOfWork;
using Vaccine.API.Models;
using Vaccine.API.Models.VaccineModel;
using Swashbuckle.AspNetCore.Filters;
using Vaccine.API.Models.ChildModel;
using Vaccine.API.Models.VaccineComboModel;
using System.Net.WebSockets;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.FileProviders;

namespace Vaccine.API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public VaccineController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var vaccines = _unitOfWork.VaccineRepository.Get();

            return Ok(vaccines);
        }

        [HttpGet("{name}")]
        public IActionResult GetVaccineByName(string name)
        {
            var vaccines = _unitOfWork.VaccineRepository.Get(v => v.Name == name).FirstOrDefault();
            if (vaccines == null)
            {
                return NotFound(new { message = "Vaccine not found." });
            }

            return Ok(vaccines);
        }

        [HttpGet("sort-by-name")]
        public IActionResult SortVaccineByName()
        {
            var vaccines = _unitOfWork.VaccineRepository.Get()
                     .OrderBy(v => v.Name)
                     .ToList();

            if (!vaccines.Any())
            {
                return NotFound(new { message = "No vaccines found." });
            }

            return Ok(vaccines);
        }

        [HttpGet("sort-by-price")]
        public IActionResult SortVaccineByPrice()
        {
            var vaccines = _unitOfWork.VaccineRepository.Get()
                    .OrderBy(v => v.Price)
                    .ToList();

            if (!vaccines.Any())
            {
                return NotFound(new { message = "No vaccines found." });
            }

            return Ok(vaccines);
        }
        [HttpPost("create-vaccine")]
        [SwaggerRequestExample(typeof(RequestCreateVaccineModel), typeof(ExampleRequestCreateVaccineModel))]
        public IActionResult CreateVaccine(RequestCreateVaccineModel newVaccine)
        {
            if (newVaccine == null)
            {
                return BadRequest(new { message = "Vaccine data is required" });
            }
            var vaccineEntity = new Vaccine.Repo.Entities.Vaccine
            {
                Name = newVaccine.Name,
                MaxLateDate = newVaccine.MaxLateDate,
                Price = newVaccine.Price,
                Description = newVaccine.Description,
                InternalDurationDoses = newVaccine.InternalDurationDoses
            };
            _unitOfWork.VaccineRepository.Insert(vaccineEntity);
            _unitOfWork.Save();
            return Ok(new { message = "Create new vaccine successfully" });
        }
        [HttpGet("get-vaccine-by-name")]
        public IActionResult GetVaccineByNameForStaff(string name)
        {
            var vaccines = _unitOfWork.VaccineRepository.Get(v => v.Name.Contains(name)).ToList();
            if (vaccines == null)
            {
                return NotFound(new { message = "Vaccine not found." });
            }

            return Ok(vaccines);
        }
        [HttpGet("get-vaccine-for-staff")]
        public IActionResult GetVaccineForStaff()
        {
            var vaccineBatchDetails = _unitOfWork.VaccineRepository.Get(
                includeProperties: "VaccineBatchDetails,VaccineBatchDetails.BatchNumberNavigation"
            ).Select(v => new
            {
                VaccineId = v.VaccineId,
                VaccineName = v.Name,
                Description = v.Description,
                Price = v.Price,
                MaxLateDate= v.MaxLateDate,
                InternaDurationDose= v.InternalDurationDoses,
                Batches = v.VaccineBatchDetails.Select(vbd => new
                {
                    BatchNumber = vbd.BatchNumber,
                    Manufacturer = vbd.BatchNumberNavigation.Manufacturer,
                    ExpiryDate = vbd.BatchNumberNavigation.ExpiryDate,
                    Quantity = vbd.Quantity
                }).ToList()
            }).ToList();

            return Ok(vaccineBatchDetails);


        }

        [HttpPut("update-vaccine/{id}")]
        [SwaggerRequestExample(typeof(RequestUpdateVaccineModel), typeof(ExampleRequestUpdateVaccineModel))]
        public IActionResult UpdateVaccine(int id, RequestUpdateVaccineModel updateVaccine)
        {
            if (updateVaccine == null)
            {
                return BadRequest(new { message = "Vaccine data is requied" });
            }
            var found = _unitOfWork.VaccineRepository.GetByID(id);
            if (found == null)
            {
                return BadRequest(new { message = "Cannot find vaccine to update" });
            }
            found.Name = updateVaccine.Name;
            found.MaxLateDate = updateVaccine.MaxLateDate;
            found.Price = updateVaccine.Price;
            found.Description = updateVaccine.Description;
            found.InternalDurationDoses = updateVaccine.InternalDurationDoses;
            _unitOfWork.VaccineRepository.Update(found);
            _unitOfWork.Save();
            return Ok(new { mesage = "Upate vaccine successfully" });

        }
        [HttpPost("create-vacccine-combo")]
        [SwaggerRequestExample(typeof(RequestCreateVaccineComboModel), typeof(ExampleRequestCreateVaccineComboModel))]
        public IActionResult CreateVaccineCombo(RequestCreateVaccineComboModel newCombo)
        {
            if (newCombo == null)
            {
                return BadRequest(new { message = "Combo data is required!" });
            }
            
            var vaccineCombo = new VaccineCombo
            {
                Name = newCombo.Name,
                Description = newCombo.Description,
                Price = newCombo.Price
            };
            _unitOfWork.VaccineComboRepository.Insert(vaccineCombo);
            _unitOfWork.Save();

            foreach (var vaccine in newCombo.VaccineIds)
            {
                var comboDetail = new VaccineComboDetail
                {
                    ComboId = vaccineCombo.ComboId,
                    VaccineId = vaccine
                };
                _unitOfWork.VaccineComboDetailRepository.Insert(comboDetail);
            }
            _unitOfWork.Save();

            return Ok(newCombo);
        }
        [HttpPut("update-vacccine-combo/{id}")]
        [SwaggerRequestExample(typeof(RequestUpdateVaccineComboModel), typeof(ExampleRequestUpdateVaccineComboModel))]
        public IActionResult UpdateVaccineCombo(int id, RequestUpdateVaccineComboModel updateCombo)
        {
            if (updateCombo == null)
            {
                return BadRequest(new { message = "Combo data is required!" });
            }
            var existingCombo = _unitOfWork.VaccineComboRepository.GetByID(id);
            if (existingCombo == null)
            {
                return NotFound(new { message = "Vaccine combo not found!" });
            }
            // Cập nhật thông tin combo
            existingCombo.Name = updateCombo.Name;
            existingCombo.Description = updateCombo.Description;
            existingCombo.Price = updateCombo.Price;
            _unitOfWork.VaccineComboRepository.Update(existingCombo);
            _unitOfWork.Save();
            //Xóa vaccine cũ trong VaccineComboDetail
            var existingVaccineComboDetais = _unitOfWork.VaccineComboDetailRepository.Get(filter: x => x.ComboId == id).ToList();

            var existingDetails = _unitOfWork.VaccineComboDetailRepository.Get(filter: x => x.ComboId == existingCombo.ComboId);
            foreach (var detail in existingDetails)
            {
                _unitOfWork.VaccineComboDetailRepository.Delete(detail);
            }
            _unitOfWork.Save();

            var newDetails = updateCombo.VaccineIds.Select(vaccineId => new VaccineComboDetail
            {
                ComboId = id,
                VaccineId = vaccineId
            }).ToList();

            foreach (var detail in newDetails)
            {
                _unitOfWork.VaccineComboDetailRepository.Insert(detail);
            }

            _unitOfWork.Save();
            return Ok(new { message = "Vaccine combo update successfully." });
        }
        [HttpGet("get-vaccine-combo")]
        public IActionResult GetVaccineCombos()
        {
            var vaccineCombos = _unitOfWork.VaccineComboRepository.Get(
                includeProperties: "VaccineComboDetails.Vaccine"
            ).ToList();

            if (vaccineCombos == null || !vaccineCombos.Any())
            {
                return NotFound(new { message = "No vaccine combos found." });
            }

            var response = vaccineCombos.Select(combo => new
            {
                ComboId = combo.ComboId,
                ComboName = combo.Name,
                Description = combo.Description,
                Price = combo.Price,
                Vaccines = combo.VaccineComboDetails.Select(detail => new
                {
                    VaccineId = detail.Vaccine.VaccineId,
                    VaccineName = detail.Vaccine.Name,
                    Description = detail.Vaccine.Description
                }).ToList()
            });

            return Ok(response);
        }


    }
}
