using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileSystemGlobbing;
using Vaccine.API.Models.VaccineBatchModel;
using Vaccine.Repo.Entities;
using Vaccine.Repo.UnitOfWork;

namespace Vaccine.API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class VaccineBatchController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        public VaccineBatchController(UnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }
        [HttpGet("get-vaccine-batch")]
        //public IActionResult GetAllVaccineBatch()
        //{
        //    var batch = _unitOfWork.VaccineBatchRepository.Get();
        //    if (batch == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(batch);

        //}
        //[HttpGet("get-all-batches-v2")]
        public IActionResult GetAllBatchesV2()
        {
            var batches = _unitOfWork.VaccineBatchRepository.Get(includeProperties: "VaccineBatchDetails.Vaccine");

            if (batches == null || !batches.Any())
            {
                return NotFound(new { message = "No vaccine batches found." });
            }

            var result = batches.Select(batch => new
            {
                BatchNumber = batch.BatchNumber,
                Manufacturer = batch.Manufacturer,
                ManufactureDate = batch.ManufactureDate,
                ExpiryDate = batch.ExpiryDate,
                Country = batch.Country,
                Status = batch.Status,
                Vaccines = (batch.VaccineBatchDetails) // Kiểm tra `null`
                    .Where(v => v.Vaccine != null) // Chỉ lấy vaccine không null
                    .Select(v => new
                    {
                        VaccineId = v.Vaccine.VaccineId,
                        VaccineName = v.Vaccine.Name, 
                        Description = v.Vaccine.Description ?? "No Description",
                        Price = v.Vaccine.Price,
                        Quantity = v.Quantity,
                        PreOrderQuantity = v.PreOrderQuantity
                    }).ToList()

            }).ToList(); 

            return Ok(result);
        }

        [HttpGet("get-vaccine-by-batch/{batchId}")]
        public IActionResult GetVaccineByBatch(string batchId)
        {
            var batchDetails = _unitOfWork.VaccineBatchDetailRepository.Get(
                v => v.BatchNumber == batchId,
                includeProperties: "Vaccine"
            );

            if (batchDetails == null || !batchDetails.Any())
            {
                return NotFound(new { message = "No vaccines found for the given batch." });
            }

            var result = batchDetails.Select(v => new
            {
                v.BatchNumber,
                VaccineName = v.Vaccine.Name,
                v.Vaccine.Description,
                v.Vaccine.Price,
                v.Quantity, 
                
              
            });

            return Ok(result);
        }
        [HttpGet("get-batch-by-vaccineID/{vaccineId}/{appointmentDate}")]
        public IActionResult GetVaccineBatchByVaccineID(int vaccineId, DateTime appointmentDate)
        {
            // loc theo dieu kien : expiredDate phai lon hon ngay di chich +3, 
            var batchDetails = _unitOfWork.VaccineBatchDetailRepository // expiryDate phai lon hon ngay hom nay
            .Get(v => v.VaccineId == vaccineId && v.BatchNumberNavigation.ExpiryDate > DateOnly.FromDateTime(appointmentDate).AddDays(3) && v.Quantity > 0, includeProperties: "Vaccine,BatchNumberNavigation")
            .OrderBy(v => v.BatchNumberNavigation.ExpiryDate)
            .ThenBy(v => v.Quantity)
            .ToList();


            if (batchDetails == null || !batchDetails.Any())
            {
                return NotFound(new { message = "No vaccines found for the given batch." });
            }

            var result = batchDetails.
              Select(v => new
              {
                  v.BatchNumber,
                  VaccineName = v.Vaccine.Name,
                  v.VaccineId,
                  v.Vaccine.Description,
                  v.Vaccine.Price,
                  v.Quantity,
                  v.PreOrderQuantity,
                  v.BatchNumberNavigation.ExpiryDate
              });


            return Ok(result);
        }



        [HttpPost("create-vaccine-batch")]
        public async Task<IActionResult> CreateVaccineBatch([FromBody] RequestCreateVaccineBatch newBatch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Trả về danh sách lỗi từ Data Annotation
            }

            var existingBatch = _unitOfWork.VaccineBatchRepository.Get(filter : x=> x.BatchNumber== newBatch.BatchNumber).FirstOrDefault();
            if (existingBatch != null)
            {
                return BadRequest("Batch number already exists.");
            }
            // Lấy danh sách VaccineId hợp lệ từ database
            var validVaccineIds = _unitOfWork.VaccineRepository
                .Get()
                .Select(v => v.VaccineId) 
                .ToHashSet();

            // Kiểm tra tất cả VaccineId có hợp lệ không trước khi tiếp tục
            foreach (var detailDto in newBatch.VaccineBatchDetails)
            {
                if (!validVaccineIds.Contains(detailDto.VaccineId))
                {
                    return BadRequest($"Vaccine ID {detailDto.VaccineId} does not exist.");
                }
            }
            var batch = new VaccineBatch
            {
                BatchNumber = newBatch.BatchNumber,
                Manufacturer = newBatch.Manufacturer,
                ManufactureDate = newBatch.ManufactureDate,
                ExpiryDate = newBatch.ExpiryDate,
                Country = newBatch.Country,
                //Duration = newBatch.Duration,
                Status = newBatch.Status
            };

            _unitOfWork.VaccineBatchRepository.Insert(batch);
            await _unitOfWork.SaveAsync();

            foreach (var detailDto in newBatch.VaccineBatchDetails)
            {
                var detail = new VaccineBatchDetail
                {
                    BatchNumber = batch.BatchNumber,
                    VaccineId = detailDto.VaccineId,
                    Quantity = detailDto.Quantity
                };
                _unitOfWork.VaccineBatchDetailRepository.Insert(detail);
            }

            await _unitOfWork.SaveAsync();
            return Ok(new { Message = "Vaccine batch created successfully", batch });
        }
        //[HttpPut("update/{batchNumber}")]
        //public async Task<IActionResult> UpdateVaccineBatch(string batchNumber, [FromBody] RequestUpdateVaccineBatch batchDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Kiểm tra xem lô có tồn tại không
        //    var existingBatch = _unitOfWork.VaccineBatchRepository
        //        .Get(filter: x => x.BatchNumber == batchNumber)
        //        .FirstOrDefault();
        //    Console.WriteLine(existingBatch);
        //    if (existingBatch == null)
        //    {
        //        return NotFound("Batch not found.");
        //    }
        //    // ko thay đổi batchNumber
        //    // Cập nhật thông tin lô vaccine
        //    existingBatch.Manufacturer = batchDto.Manufacturer;
        //    existingBatch.ManufactureDate = batchDto.ManufactureDate;
        //    existingBatch.ExpiryDate = batchDto.ExpiryDate;
        //    existingBatch.Country = batchDto.Country;
        //   // existingBatch.Duration = batchDto.Duration;
        //    existingBatch.Status = batchDto.Status;
        //    // Lấy danh sách VaccineId hợp lệ từ database
        //    var validVaccineIds = _unitOfWork.VaccineRepository
        //        .Get()
        //        .Select(v => v.VaccineId)
        //        .ToHashSet();

        //    // Kiểm tra tất cả VaccineId có hợp lệ không trước khi tiếp tục
        //    foreach (var detailDto in batchDto.VaccineBatchDetails)
        //    {
        //        if (!validVaccineIds.Contains(detailDto.VaccineId))
        //        {
        //            return BadRequest($"Vaccine ID {detailDto.VaccineId} does not exist.");
        //        }
        //    }
        //    _unitOfWork.VaccineBatchRepository.Update(existingBatch);
        //    await _unitOfWork.SaveAsync();

        //    // Xóa chi tiết vaccine cũ và thêm mới
        //    var existingDetails = _unitOfWork.VaccineBatchDetailRepository
        //        .Get(filter: x => x.BatchNumber == batchNumber)
        //        .ToList();

        //    foreach (var detail in existingDetails)
        //    {
        //        _unitOfWork.VaccineBatchDetailRepository.Delete(detail);
        //    }

        //    await _unitOfWork.SaveAsync(); // Lưu thay đổi

        //    // Thêm danh sách vaccine mới
        //    foreach (var detailDto in batchDto.VaccineBatchDetails)
        //    {
        //        var newDetail = new VaccineBatchDetail
        //        {
        //            BatchNumber = batchNumber,
        //            VaccineId = detailDto.VaccineId,
        //            Quantity = detailDto.Quantity
        //        };
        //        _unitOfWork.VaccineBatchDetailRepository.Insert(newDetail);
        //    }

        //    await _unitOfWork.SaveAsync();

        //    return Ok(new { Message = "Vaccine batch updated successfully", batch = existingBatch });
        //}
        [HttpPut("update/{batchNumber}")]
        public async Task<IActionResult> UpdateVaccineBatch(string batchNumber, [FromBody] RequestUpdateVaccineBatch batchDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra xem lô có tồn tại không
            var existingBatch = _unitOfWork.VaccineBatchRepository
                .Get(filter: x => x.BatchNumber == batchNumber)
                .FirstOrDefault();
            if (existingBatch == null)
            {
                return NotFound("Batch not found.");
            }

            // Cập nhật thông tin lô vaccine
            existingBatch.Manufacturer = batchDto.Manufacturer;
            existingBatch.ManufactureDate = batchDto.ManufactureDate;
            existingBatch.ExpiryDate = batchDto.ExpiryDate;
            existingBatch.Country = batchDto.Country;
            existingBatch.Status = batchDto.Status;

            _unitOfWork.VaccineBatchRepository.Update(existingBatch);
            await _unitOfWork.SaveAsync();

            // Lấy danh sách VaccineId hợp lệ từ database
            var validVaccineIds = _unitOfWork.VaccineRepository
                .Get()
                .Select(v => v.VaccineId)
                .ToHashSet();

            // Kiểm tra tất cả VaccineId có hợp lệ không trước khi tiếp tục
            foreach (var detailDto in batchDto.VaccineBatchDetails)
            {
                if (!validVaccineIds.Contains(detailDto.VaccineId))
                {
                    return BadRequest($"Vaccine ID {detailDto.VaccineId} does not exist.");
                }
            }

            // Lấy danh sách vaccine cũ trong batch để giữ lại `PreOrderQuantity`
            var existingDetails = _unitOfWork.VaccineBatchDetailRepository
                .Get(filter: x => x.BatchNumber == batchNumber)
                .ToList();

            // Cập nhật danh sách vaccine trong batch mà không làm mất `PreOrderQuantity`
            foreach (var detailDto in batchDto.VaccineBatchDetails)
            {
                var existingDetail = existingDetails
                    .FirstOrDefault(x => x.VaccineId == detailDto.VaccineId);

                if (existingDetail != null)
                {
                    // Nếu vaccine đã tồn tại, cập nhật số lượng nhưng giữ nguyên `PreOrderQuantity`
                    existingDetail.Quantity = detailDto.Quantity;
                    _unitOfWork.VaccineBatchDetailRepository.Update(existingDetail);
                }
                else
                {
                    // Nếu vaccine chưa tồn tại, thêm mới với `PreOrderQuantity = 0`
                    var newDetail = new VaccineBatchDetail
                    {
                        BatchNumber = batchNumber,
                        VaccineId = detailDto.VaccineId,
                        Quantity = detailDto.Quantity,
                        PreOrderQuantity = 0 // Hoặc gán từ giá trị mặc định
                    };
                    _unitOfWork.VaccineBatchDetailRepository.Insert(newDetail);
                }
            }

            await _unitOfWork.SaveAsync();

            return Ok(new { Message = "Vaccine batch updated successfully", batch = existingBatch });
        }

        [HttpPost("link-vaccine-to-batch")]

        public async Task<IActionResult> LinkVaccineToBatch([FromBody] LinkVaccineToBatchRequest request)
        {
            // Kiểm tra dữ liệu đầu vào hợp lệ
            if (request == null || request.VaccineId <= 0 || request.Quantity <= 0 || string.IsNullOrEmpty(request.BatchId))
            {
                return BadRequest(new { message = "Invalid input data." });
            }

            // Kiểm tra batch có tồn tại không
            var batch = _unitOfWork.VaccineBatchRepository.Get(filter: b => b.BatchNumber == request.BatchId).FirstOrDefault();
            if (batch == null)
            {
                return NotFound(new { message = "Batch not found." });
            }

            // Kiểm tra vaccine có tồn tại không
            var vaccine = _unitOfWork.VaccineRepository.Get(filter: v => v.VaccineId == request.VaccineId).FirstOrDefault();
            if (vaccine == null)
            {
                return NotFound(new { message = "Vaccine not found." });
            }

            // Kiểm tra xem vaccine đã được liên kết với batch này chưa
            var existingDetail = _unitOfWork.VaccineBatchDetailRepository
                .Get(filter: vbd => vbd.BatchNumber == request.BatchId && vbd.VaccineId == request.VaccineId)
                .FirstOrDefault();

            if (existingDetail != null)
            {
                // Nếu vaccine đã có trong batch, cộng dồn số lượng
                existingDetail.Quantity += request.Quantity;
                _unitOfWork.VaccineBatchDetailRepository.Update(existingDetail);
            }
            else
            {
                // Nếu chưa có, tạo mới liên kết
                var newDetail = new VaccineBatchDetail
                {
                    BatchNumber = request.BatchId,
                    VaccineId = request.VaccineId,
                    Quantity = request.Quantity
                };
                _unitOfWork.VaccineBatchDetailRepository.Insert(newDetail);
            }

            await _unitOfWork.SaveAsync(); // Lưu thay đổi vào database

            return Ok(new { message = "Vaccine linked to batch successfully." });
        }
    }
   

}
