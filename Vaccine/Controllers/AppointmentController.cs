using Azure.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkStore.API.Models.CustomerModel;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net.WebSockets;
using System.Reflection.PortableExecutable;
using System.Runtime;
using Vaccine.API.Helper;
using Vaccine.API.Models.AppointmentModel;
using Vaccine.API.Models.CustomerModel;
using Vaccine.API.Models.EmailModel;
using Vaccine.API.Models.InvoiceDetailModel;
using Vaccine.API.Models.InvoiceModel;
using Vaccine.Repo.Entities;
using Vaccine.Repo.Repository;
using Vaccine.Repo.UnitOfWork;

namespace Vaccine.API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public AppointmentController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("get-appointments")]
        public IActionResult GetAppointments()
        {
            var appointments = _unitOfWork.AppointmentRepository.Get();
            return Ok(appointments);
        }

        [HttpGet("get-appointment/{id}")]
        public IActionResult GetAppointment(int id)
        {
            var appointment = _unitOfWork.AppointmentRepository.GetByID(id);
            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found." });
            }
            return Ok(appointment);
        }

        [HttpGet("get-appointment-by-childid/{child_id}")]
        public IActionResult GetAppointmentsByChildId(int child_id)
        {
            var appointments = _unitOfWork.AppointmentRepository
                .Get(filter: a => a.ChildId == child_id);

            if (!appointments.Any())
            {
                return NotFound(new { message = "No appointments found for this child." });
            }
            return Ok(appointments);
        }
        [HttpGet("get-appointment-checkin")]
        public IActionResult GetAppointmentsForCheckin()
        {

            var allAppointments = _unitOfWork.AppointmentRepository.Get().ToList();

            var appointments = allAppointments
                .Where(x => (x.Status.Trim().ToLower() == "late" || (x.Status.Trim().ToLower() == "approved"
                            && x.AppointmentDate.ToLocalTime().Date == DateTime.Today.Date)))
                .ToList();

            if (appointments == null)
            {
                return NotFound(new { message = "No appointments found for today" });
            }
            return Ok(appointments);
        }
        [HttpPatch("set-appointment-inprogress/{appointmentId}")]
        public IActionResult SetAppointmentInprogress(int appointmentId)
        {
            var appointment = _unitOfWork.AppointmentRepository.GetByID(appointmentId);
            if (appointment == null)
            {
                return NotFound(new { message = "Cannot find appointment" });
            }
            appointment.Status = "InProgress";
            _unitOfWork.AppointmentRepository.Update(appointment);
            _unitOfWork.Save();
            return Ok(new { message = $"Sucessfully update status of appointment: {appointment.AppointmentId}" });
        }
        [HttpGet("get-appointment-pending")]
        public IActionResult GetAppointmentPenidng()
        {
            var appointments = _unitOfWork.AppointmentRepository.Get(filter: x => x.Status == "Pending");
            if (appointments.Count() == 0)
            {
                return Ok(new { message = "No pending appointments found", appointments = new List<Appointment>() });
            }
            return Ok(appointments);
        }
        [HttpGet("get-appointment-in-progress")]
        public IActionResult GetAppointmentsInProgress()
        {

            var allAppointments = _unitOfWork.AppointmentRepository.Get().ToList();

            var appointments = allAppointments
                .Where(x => x.Status=="InProgress")
                .ToList();

            if (appointments == null)
            {
                return NotFound(new { message = "No appointments found for today" });
            }
            return Ok(appointments);
        }
       

        [HttpPut("Approved-status-appointment/{id}/{batchNumber}")]
        [SwaggerOperation(
            Description = "Confirm an appointment by setting the status to Approved."
        )]
        public IActionResult ApprovedAppointment(int id, string batchNumber)
        {
            if (string.IsNullOrEmpty(batchNumber) || id==null)
            {
                return BadRequest(new { message = "Batch number and appointmentID is required." });
            }
            var appointment = _unitOfWork.AppointmentRepository.GetByID(id);
            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found." });
            }
            // Ensure the appointment is not already confirmed
            if (appointment.Status == "Approved")
            {
                return BadRequest(new { message = "Appointment is already confirmed." });
            }
            var batchPreOrderQuanity = _unitOfWork.VaccineBatchDetailRepository.Get(filter: x => x.BatchNumber == batchNumber && x.VaccineId == appointment.VaccineId).FirstOrDefault();

            if (string.IsNullOrEmpty(batchNumber))
            {
                return BadRequest(new { message = "Appointment batch number is missing" });
            }
            if (batchPreOrderQuanity.Quantity <= 0)
            {
                return BadRequest(new { message = "Not enough quanity in batch for this vaccine." });
            }
            if (batchPreOrderQuanity.PreOrderQuantity >= batchPreOrderQuanity.Quantity)
            {
                return BadRequest(new { message = "Not enough available quantity in the batch for this vaccine." });
            }

            // Update the appointment status to Confirmed
            appointment.Status = "Approved";
            //appointment.BatchNumber = batchNumber;
            batchPreOrderQuanity.PreOrderQuantity += 1;
            _unitOfWork.AppointmentRepository.Update(appointment);
            _unitOfWork.VaccineBatchDetailRepository.Update(batchPreOrderQuanity);
            _unitOfWork.Save();
            return Ok(new { message = "Appointment approved successfully." });
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

        

        [HttpPost("create-appointment-combo")]
        [SwaggerOperation(
            Description = "Create appointment when purchase combo"
        )]
        public IActionResult CreateAppointmentsCombo(RequestCreateComboAppointment request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Combo data is required" });
            }
            var comboDetails = _unitOfWork.VaccineComboDetailRepository.Get(filter: x => x.ComboId == request.ComboId).ToList();
            if (comboDetails == null)
            {
                return BadRequest(new { message = "ComboId is not found " });
            }
            if (!comboDetails.Any())
            {
                return BadRequest(new { message = "ComboId does not contain any vaccine" });
            }
            var appointmentList = new List<Appointment>();
            DateTime currentAppointmentDate = request.AppointmentDate;
            var selectedBatches = new Dictionary<int, List<VaccineBatchDetail>>();
            bool pendingFound = false;
            foreach (var detail in comboDetails)
            {
                // tìm kiến vaccine theo vaccineID
                var vaccine = _unitOfWork.VaccineRepository.Get(filter: x => x.VaccineId == detail.VaccineId).FirstOrDefault();
                if (vaccine == null)
                {
                    return BadRequest(new { message = $"Vaccine Error: {detail.VaccineId} does not contain internalDurationDay" });
                }
                // từ vaccineID => truy xuất ra ngày cần tiêm tiếp theo
                int durationDays = (int)vaccine.InternalDurationDoses;
                // Kiểm tra tổng số vaccine theo vaccineID trong kho
                var totalStock = _unitOfWork.VaccineBatchDetailRepository.Get(
                    v => v.VaccineId == detail.VaccineId && v.BatchNumberNavigation.ExpiryDate > DateOnly.FromDateTime(currentAppointmentDate).AddDays((int)vaccine.MaxLateDate) && v.BatchNumberNavigation.Status.ToLower() == "available", includeProperties: "BatchNumberNavigation"
                ).Sum(v => v.Quantity);

                //if (totalStock < 10)
                //{
                //    return BadRequest(new { message = $"Insufficient stock for vaccine {detail.VaccineId}. Appointment combo cannot be scheduled." });
                //}
                // tạo mới appointment cho lần tiêm tiếp theo
                string status = totalStock >= 10 ? "Approved" : "Pending";
                if (status == "Pending")
                {
                    pendingFound = true;
                }
                var appointment = new Appointment
                {
                    AppointmentDate = currentAppointmentDate,
                    Status = status,
                    Notes = request.Notes,
                    CreatedAt = DateTime.Now,
                    ChildId = request.ChildId,
                    StaffId = null,  // Nhân viên xử lý nếu Pending
                    DoctorId = null,
                    VaccineType = "Combo",
                    ComboId = request.ComboId,
                    CustomerId = request.CustomerId,
                    VaccineId = detail.VaccineId
                };
                if (totalStock >= 10 && pendingFound == false)
                {
                    // chọn batch của vaccine với điều kiện( gần hết ngày hết hạn nhất & số lượng còn ít trước)
                    var availableBatch = _unitOfWork.VaccineBatchDetailRepository.Get(x => x.VaccineId == vaccine.VaccineId && x.Quantity > 0 && x.BatchNumberNavigation.ExpiryDate > DateOnly.FromDateTime(currentAppointmentDate.Date.AddDays((double)vaccine.MaxLateDate)) && x.BatchNumberNavigation.Status.ToLower()=="available", includeProperties: "BatchNumberNavigation")
                      .OrderBy(v => v.BatchNumberNavigation.ExpiryDate)
                      .ThenBy(v => v.Quantity)
                      .FirstOrDefault();
                    if (availableBatch != null)
                    {
                        //appointment.BatchNumber = availableBatch.BatchNumber;
                        if (!selectedBatches.ContainsKey(detail.VaccineId))
                        {
                            selectedBatches[detail.VaccineId] = new List<VaccineBatchDetail>();
                        }
                        selectedBatches[detail.VaccineId].Add(availableBatch);
                    }

                }
                appointmentList.Add(appointment);

                currentAppointmentDate = currentAppointmentDate.AddDays(durationDays);
            }
            if (pendingFound)
            {
                foreach (var appt in appointmentList)
                {
                    appt.Status = "Pending";
                }
            }
            else
            {
                foreach (var batchList in selectedBatches.Values)
                {
                    foreach (var batch in batchList)
                    {
                        batch.PreOrderQuantity++;
                        _unitOfWork.VaccineBatchDetailRepository.Update(batch);
                    }
                }
            }
            _unitOfWork.AppointmentRepository.InsertRange(appointmentList);
            _unitOfWork.Save();
            //---------------------------------------------------------------------------------------
            // Tạo Invoice trạng thái Unpaid
            //----------------------------------------------------------------------------------------
            //  Gửi email nếu tất cả lịch hẹn đều được phê duyệt
            if (appointmentList.All(a => a.Status == "Approved"))
            {

                SendAppointmentConfirmationEmail(request.CustomerId, appointmentList);
            }
            //------------------------------------------------------------------------
            // tạo invoice status pending
            Invoice? invoice = null;
            if (pendingFound)
            {
                var comboPrice = _unitOfWork.VaccineComboRepository
                                .Get(filter: c => c.ComboId == request.ComboId)
                                .FirstOrDefault()?.Price ?? 0;
                invoice = CreateInvoice(new RequestCreateInvoiceModel
                {
                    CustomerId = request.CustomerId,
                    Type = "Combo",
                    TotalAmount = comboPrice,// Lấy giá combo
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });

                foreach (var appointment in appointmentList.Where(a => a.Status == "Pending"))
                {
                    var vaccinePrice = _unitOfWork.VaccineRepository
                                        .Get(filter: x => x.VaccineId == appointment.VaccineId)
                                        .FirstOrDefault()?.Price ?? 0;
                    CreateInvoiceDetail(new RequestCreateInvoiceDetailModel
                    {
                        InvoiceId = invoice.InvoiceId,
                        VaccineId = appointment.VaccineId,
                        AppointmentId = appointment.AppointmentId,
                        ComboId = request.ComboId,
                        Quantity = 1,
                        Price = vaccinePrice // Không cần giá vì đã set trong Invoice
                    });
                }
            }



            return Ok(new
            {
                message = appointmentList.All(a => a.Status == "Approved") ?
           "Tất cả lịch hẹn trong combo đã được chấp nhận." :
           "Một số lịch hẹn đang chờ xác nhận từ nhân viên.",
                Appointments = appointmentList
            });

        }

        [HttpPost("create-appointment")]
        [SwaggerOperation(Description = "Create appointment single")]
        public IActionResult CreateAppointment(RequestCreateAppointmentModel request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Appointment data is required !" });
            }

            var vaccine = _unitOfWork.VaccineRepository
                .Get(x => x.VaccineId == request.VaccineId)
                .FirstOrDefault();

            if (vaccine == null)
            {
                return BadRequest(new { message = $"Vaccine with id {request.VaccineId} is not found !" });
            }

            var appointEntity = new Appointment
            {
                AppointmentDate = request.AppointmentDate,
                Status = "Approved",
                Notes = request.Notes,
                CreatedAt = request.CreatedAt,
                ChildId = request.ChildId,
                StaffId = null,
                DoctorId = null,
                VaccineType = "Single",
                ComboId = null,
                CustomerId = request.CustomerId,
                VaccineId = request.VaccineId,
            };

            _unitOfWork.AppointmentRepository.Insert(appointEntity);
            _unitOfWork.Save(); // cần lưu để có AppointmentId

            // Tạo invoice
            var invoice = new Invoice
            {
                CustomerId = request.CustomerId,
                TotalAmount = vaccine.Price, // dùng giá vaccine
                Status = "Unpaid",
                Type = "Single",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _unitOfWork.InvoiceRepository.Insert(invoice);
            _unitOfWork.Save(); // cần lưu để có InvoiceId

            // Tạo invoice detail
            var invoiceDetail = new InvoiceDetail
            {
                InvoiceId = invoice.InvoiceId,
                VaccineId = request.VaccineId,
                AppointmentId = appointEntity.AppointmentId,
                ComboId = null,
                Quantity = 1,
                Price = vaccine.Price
            };

            _unitOfWork.InvoiceDetailRepository.Insert(invoiceDetail);
            _unitOfWork.Save();

            var responseAppoint = new RequestResponeAppointmentModel
            {
                AppointmentId = appointEntity.AppointmentId,
                AppointmentDate = request.AppointmentDate,
                Status = "Approved",
                Notes = request.Notes,
                CreatedAt = request.CreatedAt,
                ChildId = request.ChildId,
                StaffId = null,
                DoctorId = null,
                VaccineType = "Single",
                ComboId = null,
                CustomerId = request.CustomerId,
                VaccineId = request.VaccineId,
            };

            return Ok(new
            {
                message = "Appointment and invoice created successfully.",
                appointment = responseAppoint,
                invoiceId = invoice.InvoiceId
            });
        }


        [HttpPut("update-status-appointment-success/{appointmentId}/{batchNumber}/{vaccineId}")]
        public IActionResult updateStatusSucess(int appointmentId, string batchNumber, int vaccineId)
        {
            if (appointmentId == null)
            {
                return BadRequest(new { message = " appointmentId is required !" });
            }
            var appointment= _unitOfWork.AppointmentRepository.Get(filter: x=> x.AppointmentId== appointmentId).FirstOrDefault();  
            if (appointment == null)
            {
                return BadRequest(new { message = "appointment is not found ." });
            }
            var batchVaccine = _unitOfWork.VaccineBatchDetailRepository
                               .Get(filter: x => x.BatchNumber == batchNumber && x.VaccineId == vaccineId)
                               .FirstOrDefault();
            if (batchVaccine == null)
            {
                return NotFound(new { message = $"Vaccine {vaccineId} not found in batch {batchNumber}." });
            }
            // Kiểm tra số lượng trước khi giảm
            if (batchVaccine.PreOrderQuantity <= 0 || batchVaccine.Quantity <= 0)
            {
                return BadRequest(new { message = $"Insufficient stock for vaccine {vaccineId} in batch {batchNumber}." });
            }
            appointment.Status = "Success";
            batchVaccine.PreOrderQuantity--;
            batchVaccine.Quantity--;
            _unitOfWork.AppointmentRepository.Update(appointment);
            _unitOfWork.VaccineBatchDetailRepository.Update(batchVaccine);
            _unitOfWork.Save();
            return Ok(new { message = "Updated success status for appointment" });

        }

        [HttpPut("update-status-appointment-rejected/{appointmentId}")]
        public IActionResult updateStatusRejected(int appointmentId)
        {
            if (appointmentId == null)
            {
                return BadRequest(new { message = " appointmentId is required !" });
            }
            var appointment = _unitOfWork.AppointmentRepository.Get(filter: x => x.AppointmentId == appointmentId).FirstOrDefault();
            if (appointment == null)
            {
                return BadRequest(new { message = "appointment is not found ." });
            }
            appointment.Status = "Rejected";
            _unitOfWork.AppointmentRepository.Update(appointment);
            _unitOfWork.Save();
            return Ok(new { message = "Updated rejected status for appointment" });
        }
        //---------------------------------------------------------------------------
        [ApiExplorerSettings(IgnoreApi = true)]
        public bool SendEmail(RequestSendEmailModel requestSendEmailModel)
        {
            // check email có đúng cú pháp không
            if (!Utils.IsValidEmail(requestSendEmailModel.Email))
            {
                Console.WriteLine("Email không hợp lệ");
                return false;
            }
            try
            {
                EmailRepository.SendEmail(requestSendEmailModel.Email, requestSendEmailModel.Subject, requestSendEmailModel.Body);
                return true;
            }catch(Exception ex) 
            {
                Console.WriteLine(ex);
                return false;
            }
                       
        }
        private void SendAppointmentConfirmationEmail(int customerId, List<Appointment> appointmentList)
        {
            var customer = _unitOfWork.CustomerRepository.GetByID(customerId);
            if (customer == null || string.IsNullOrEmpty(customer.Email))
            {
                Console.WriteLine("Không thể gửi email: Khách hàng không tồn tại hoặc không có email.");
                return;
            }

            // 🔥 Tạo danh sách lịch hẹn theo bảng HTML
            string appointmentDetails = "";
            foreach (var appointment in appointmentList)
            {
                var vaccine = appointment.VaccineId.HasValue
                                ? _unitOfWork.VaccineRepository.GetByID(appointment.VaccineId.Value)
                                : null;
                string vaccineName = vaccine != null ? vaccine.Name : "Vaccine không xác định";

                appointmentDetails += $@"
            <tr>
                <td><b>{vaccineName}</b></td>
                <td>{appointment.AppointmentDate:dd/MM/yyyy}</td>
                <td>{appointment.Status}</td>
            </tr>";
            }

            // 🔥 Tạo nội dung email
            string emailBody = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    line-height: 1.6;
                    color: #333;
                    background-color: #f4f4f4;
                    padding: 20px;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    background: #ffffff;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
                }}
                h2 {{
                    color: #007bff;
                }}
                table {{
                    width: 100%;
                    border-collapse: collapse;
                    margin-top: 15px;
                    background: #fff;
                }}
                th, td {{
                    border: 1px solid #ddd;
                    padding: 10px;
                    text-align: left;
                }}
                th {{
                    background-color: #007bff;
                    color: white;
                    text-align: center;
                }}
                .footer {{
                    margin-top: 20px;
                    padding-top: 15px;
                    border-top: 1px solid #ddd;
                    font-size: 12px;
                    color: #666;
                    text-align: center;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h2>Xin chào {(string.IsNullOrEmpty(customer.Name) ? "bạn" : customer.Name)},</h2>
                <p>Lịch hẹn tiêm chủng của bạn đã được xác nhận. Dưới đây là lịch tiêm chi tiết:</p>
                <table>
                    <tr>
                        <th>Vaccine</th>
                        <th>Ngày tiêm</th>
                        <th>Trạng thái</th>
                    </tr>
                    {appointmentDetails}
                </table>
                <p class='footer'>
                    Cảm ơn bạn đã tin tưởng dịch vụ của chúng tôi. Nếu có bất kỳ câu hỏi nào, vui lòng liên hệ trung tâm y tế.
                </p>
            </div>
        </body>
        </html>";

            // 🔥 Tạo email request
            var emailRequest = new RequestSendEmailModel
            {
                Email = customer.Email,
                Subject = "Xác nhận lịch hẹn tiêm chủng",
                Body = emailBody
            };

            try
            {
                bool emailSent = SendEmail(emailRequest);
                if (!emailSent)
                {
                    Console.WriteLine("Gửi email thất bại!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Lỗi khi gửi email: {ex.Message}");
            }
        }

        //----------------------------------------------------------------------------
        [ApiExplorerSettings(IgnoreApi = true)]
        public Invoice CreateInvoice(RequestCreateInvoiceModel requestCreateInvoiceModel)
        {
            //if (requestCreateInvoiceModel.CustomerId == 0)
            //{
            //    return BadRequest(new { message = "Customer ID is required." });
            //}
            var invoiceEntity = new Invoice
            {
                CustomerId = requestCreateInvoiceModel.CustomerId,
                TotalAmount = requestCreateInvoiceModel.TotalAmount,
                Status = "Pending",
                Type = requestCreateInvoiceModel.Type,
                CreatedAt = requestCreateInvoiceModel.CreatedAt,
                UpdatedAt = requestCreateInvoiceModel.UpdatedAt

            };
            _unitOfWork.InvoiceRepository.Insert(invoiceEntity);
            _unitOfWork.Save();
            return invoiceEntity;
        }
        //----------------------------------------------------------------------------------
        // nay tao invoice va invoice details
        [ApiExplorerSettings(IgnoreApi = true)]
        public InvoiceDetail CreateInvoiceDetail(RequestCreateInvoiceDetailModel requestCreateInvoiceDetailModel)
        {
            //if (requestCreateInvoiceDetailModel.Quantity == null || requestCreateInvoiceDetailModel.Quantity <= 0)
            //{
            //    return BadRequest("Quantity must be greater than 0.");
            //}

            var invoiceDetailEntity = new InvoiceDetail
            {
                InvoiceId = requestCreateInvoiceDetailModel.InvoiceId,
                VaccineId = requestCreateInvoiceDetailModel.VaccineId,
                AppointmentId = requestCreateInvoiceDetailModel.AppointmentId,
                ComboId = requestCreateInvoiceDetailModel.ComboId,
                Quantity = requestCreateInvoiceDetailModel.Quantity,
                Price = requestCreateInvoiceDetailModel.Price
            };
            _unitOfWork.InvoiceDetailRepository.Insert(invoiceDetailEntity);
            _unitOfWork.Save();
            return invoiceDetailEntity;
        }
    }
}
