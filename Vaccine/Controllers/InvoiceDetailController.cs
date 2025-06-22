using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Vaccine.API.Models.InvoiceDetailModel;
using Vaccine.Repo.Entities;
using Vaccine.Repo.UnitOfWork;

namespace Vaccine.API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceDetailController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;

        public InvoiceDetailController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("get-invoice-details")]
        public IActionResult GetInvoiceDetails()
        {
            var invoiceDetails = _unitOfWork.InvoiceDetailRepository.Get();
            return Ok(invoiceDetails);
        }

        [HttpGet("get-invoice-detail/{id}")]
        public IActionResult GetInvoiceDetail(int id)
        {
            var invoiceDetail = _unitOfWork.InvoiceDetailRepository.GetByID(id);
            if (invoiceDetail == null)
            {
                return NotFound(new { message = "Invoice detail not found." });
            }
            return Ok(invoiceDetail);
        }

        [HttpGet("get-invoice/{invoiceId}")]
        public IActionResult GetInvoice(int invoiceId)
        {
            var invoiceDetails = _unitOfWork.InvoiceRepository.Get(filter: d => d.InvoiceId == invoiceId);
            if (invoiceDetails == null || !invoiceDetails.Any())
            {
                return NotFound(new { message = "No invoice details found for this invoice." });
            }
            return Ok(invoiceDetails);
        }



        [HttpPost("create-invoice-detail")]
        [SwaggerOperation(
        Description = "invoice_id | vaccine id | appointment id | quantity để trừ ra | price của vaccine | nếu mua nhiều vaccine thì thêm nhiều cùng 1 invoice_id"
        )]  
        public IActionResult CreateInvoiceDetail(RequestCreateInvoiceDetailModel requestCreateInvoiceDetailModel)
        {
            if (requestCreateInvoiceDetailModel.Quantity == null || requestCreateInvoiceDetailModel.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0.");
            }

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
            return Ok(invoiceDetailEntity);
        }
    }
}
