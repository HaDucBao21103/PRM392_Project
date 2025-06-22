using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Vaccine.API.Models.CustomerModel;
using Vaccine.API.Models.InvoiceModel;
using Vaccine.Repo.Entities;
using Vaccine.Repo.UnitOfWork;

namespace Vaccine.API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public InvoiceController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("get-invoices")]
        public IActionResult GetInvoices()
        {
            var invoices = _unitOfWork.InvoiceRepository.Get();
            return Ok(invoices);
        }

        [HttpGet("get-invoice/{id}")]
        public IActionResult GetInvoice(int id)
        {
            var invoice = _unitOfWork.InvoiceRepository.GetByID(id);
            if (invoice == null)
            {
                return NotFound(new { message = "Invoice not found." });
            }
            return Ok(invoice);
        }

        [HttpPost("create-invoice")]
        [SwaggerOperation(
        Description = "customerID \n"
            + " totalAmount: tổng tiền\n"
            + " status: unpaid\n"
            + " type: single or combo\n")]
        public IActionResult CreateInvoice(RequestCreateInvoiceModel requestCreateInvoiceModel)
        {
            if(requestCreateInvoiceModel.CustomerId == 0)
            {
                return BadRequest(new { message = "Customer ID is required." });
            }
            var invoiceEntity = new Invoice
            {
                CustomerId = requestCreateInvoiceModel.CustomerId ,
                TotalAmount = requestCreateInvoiceModel.TotalAmount,
                Status = "Unpaid",
                Type = requestCreateInvoiceModel.Type,
                CreatedAt = requestCreateInvoiceModel.CreatedAt,
                UpdatedAt = requestCreateInvoiceModel.UpdatedAt

            };
            _unitOfWork.InvoiceRepository.Insert(invoiceEntity);
            _unitOfWork.Save();
            return Ok(invoiceEntity);
        }

        [HttpPost("create-invoice1")]
        [SwaggerOperation(
        Description = "customerID \n"
            + " totalAmount: tổng tiền\n"
            + " status: unpaid\n"
            + " type: single or combo\n")]
        public IActionResult CreateInvoice1(RequestCreateInvoiceModel1 requestCreateInvoiceModel)
        {
            if (requestCreateInvoiceModel.CustomerId ==  null)
            {
                return BadRequest(new { message = "Customer ID is required." });
            }

            // Determine type (Single or Combo)
            bool hasCombo = requestCreateInvoiceModel.InvoiceDetails.Any(d => d.ComboId != null);

            string invoiceType = hasCombo ? "Combo" : "Single";

            var invoiceEntity = new Invoice
            {
                CustomerId = requestCreateInvoiceModel.CustomerId,
                TotalAmount = requestCreateInvoiceModel.TotalAmount,
                Status = "Unpaid",
                Type = invoiceType,
                CreatedAt = requestCreateInvoiceModel.CreatedAt,
                UpdatedAt = requestCreateInvoiceModel.UpdatedAt

            };
            _unitOfWork.InvoiceRepository.Insert(invoiceEntity);
            _unitOfWork.Save();

            // Insert Invoice Details
            var invoiceDetails = requestCreateInvoiceModel.InvoiceDetails.Select(detail => new InvoiceDetail
            {
                InvoiceId = invoiceEntity.InvoiceId,
                VaccineId = detail.VaccineId,
                AppointmentId = detail.AppointmentId,
                ComboId = detail.ComboId,
                Quantity = detail.Quantity,
                Price = detail.Price
            }).ToList();

            foreach (var detail in invoiceDetails)
            {
                _unitOfWork.InvoiceDetailRepository.Insert(detail);
            }

            _unitOfWork.Save();
            // Return Invoice with its Details
            return Ok(new InvoiceDto
            {
                InvoiceId = invoiceEntity.InvoiceId,
                CustomerId = invoiceEntity.CustomerId,
                TotalAmount = invoiceEntity.TotalAmount,
                Status = invoiceEntity.Status,
                Type = invoiceEntity.Type,
                CreatedAt = invoiceEntity.CreatedAt,
                InvoiceDetails = invoiceDetails.Select(d => new InvoiceDetailDto
                {
                    VaccineId = d.VaccineId,
                    AppointmentId = d.AppointmentId,
                    ComboId = d.ComboId,
                    Quantity = d.Quantity,
                    Price = d.Price
                }).ToList()
            });
        }

        //============================================================================
        [HttpPut("update-invoice-status-to-paid/{invoice_id}")]
        [SwaggerOperation(
        Summary = "Update status of invoice to paid"        
    )]
        public IActionResult UpdateInvoiceStatusToPaid(int invoice_id)
        {
            var invoice = _unitOfWork.InvoiceRepository.GetByID(invoice_id);
            if (invoice == null)
            {
                return NotFound(new { message = "Invoice not found." });
            }

            if (invoice.Status == "Paid")
            {
                return BadRequest(new { message = "Invoice is already paid." });
            }

            // Update invoice status
            invoice.Status = "Paid";
            _unitOfWork.InvoiceRepository.Update(invoice);

            //// Get purchased vaccines from InvoiceDetail (assuming InvoiceDetail stores vaccine purchases)
            //var invoiceDetails = _unitOfWork.InvoiceDetailRepository.Get(d => d.InvoiceId == invoice_id);

            //foreach (var detail in invoiceDetails)
            //{
            //    var vaccineBatch = _unitOfWork.VaccineBatchDetailRepository
            //        .Get(vb => vb.VaccineId == detail.VaccineId)
            //        .OrderBy(vb => vb.BatchNumber)
            //        .FirstOrDefault();

            //    if (vaccineBatch == null || vaccineBatch.Quantity < detail.Quantity)
            //    {
            //        return BadRequest(new { message = $"Insufficient stock for vaccine ID {detail.VaccineId}" });
            //    }

            //    // Deduct vaccine quantity
            //    vaccineBatch.Quantity -= detail.Quantity;
            //    _unitOfWork.VaccineBatchDetailRepository.Update(vaccineBatch);
            //}

            _unitOfWork.Save();
            return Ok(new { message = "Invoice updated to Paid " });
        }
        //======================================================================================
        // nay cua nguyen 
        [HttpGet("pending-invoices")]
        public IActionResult GetPendingInvoices()
        {
            var pendingInvoices = _unitOfWork.InvoiceRepository
            .Get(filter: i => i.Status == "Pending", includeProperties: "InvoiceDetails.Appointment")
            .Select(invoice => new
            {
                InvoiceId = invoice.InvoiceId,
                CustomerId = invoice.CustomerId,
                TotalAmount = invoice.TotalAmount,
                Status = invoice.Status,
                Type = invoice.Type,
                CreatedAt = invoice.CreatedAt,
                InvoiceDetails = invoice.InvoiceDetails.Select(detail => new
                {
                    AppointmentId = detail.AppointmentId,
                    AppointmentDate = detail.Appointment != null ? detail.Appointment.AppointmentDate : (DateTime?)null, // ✅ Lấy ngày hẹn
                    VaccineId = detail.VaccineId,
                    ComboId = detail.ComboId,
                    Quantity = detail.Quantity,
                    Price = detail.Price
                }).ToList()
            })
            .ToList();

            if (!pendingInvoices.Any())
            {
                return NotFound(new { message = "No pending invoices found." });
            }

            return Ok(pendingInvoices);
        }
        [HttpPut("update-invoice-status-unpaid/{invoiceId}")]
        public IActionResult UpdateInvoiceStatusUnPaid(int invoiceId)
        {
            var invoice = _unitOfWork.InvoiceRepository.GetByID(invoiceId);

            if (invoice == null)
            {
                return NotFound(new { message = "Invoice not found." });
            }

            if (invoice.Status != "Pending")
            {
                return BadRequest(new { message = "Only invoices with 'Pending' status can be updated to 'Unpaid'." });
            }

            invoice.Status = "Unpaid"; //Cập nhật trạng thái
            invoice.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.InvoiceRepository.Update(invoice);
            _unitOfWork.Save();

            return Ok(new { message = "Invoice status updated to 'Unpaid' successfully.", Invoice = invoice });
        }
        [HttpPut("update-invoice-status-canceled/{invoiceId}")]
        public IActionResult UpdateInvoiceStatusCanceled (int invoiceId)
        {
            var invoice = _unitOfWork.InvoiceRepository.GetByID(invoiceId);

            if (invoice == null)
            {
                return NotFound(new { message = "Invoice not found." });
            }
            invoice.Status = "Cancelled"; //Cập nhật trạng thái
            invoice.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.InvoiceRepository.Update(invoice);
            _unitOfWork.Save();

            return Ok(new { message = "Invoice status updated to 'Canceled' successfully.", Invoice = invoice });
        }


    }
}
