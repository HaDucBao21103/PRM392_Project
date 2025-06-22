using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VNPAY.NET;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;
using Vaccine.Repo.UnitOfWork;
using Microsoft.AspNetCore.Cors;
using Azure.Core;

namespace Vaccine.API.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class VnPayController : ControllerBase
    {
        private readonly IVnpay _vnpay;
        private readonly IConfiguration _config;
        private readonly UnitOfWork _unitOfWork;
        private readonly ILogger<VnPayController> _logger;

        public VnPayController(IVnpay vnpay, IConfiguration configuration, UnitOfWork unitOfWork, ILogger<VnPayController> logger)
        {
            _vnpay = vnpay;
            _config = configuration;
            _unitOfWork = unitOfWork;
            _vnpay.Initialize(_config["Vnpay:TmnCode"],
                  _config["Vnpay:HashSecret"],
                  _config["Vnpay:BaseUrl"],
                  _config["Vnpay:CallbackUrl"]); // Bổ sung IPN URL
            _logger = logger;
        }

        [HttpGet("CreatePaymentUrl")]
        public ActionResult<string> CreatePaymentUrl(double moneyToPay, string description, int invoiceId)
        {
            try
            {
                var invoice = _unitOfWork.InvoiceRepository.GetByID(invoiceId);
                var ipAddress = NetworkHelper.GetIpAddress(HttpContext);

                if (invoice == null)
                {
                    return NotFound("Invoice not found.");
                }

                if (invoice.Status == "Paid")
                {
                    return BadRequest("Invoice has already been paid.");
                }
                var request = new PaymentRequest
                {
                    PaymentId = invoiceId, // Use the invoiceId as the PaymentId for tracking
                    //PaymentId = DateTime.Now.Ticks,
                    Money = moneyToPay,
                    Description = description,
                    IpAddress = ipAddress,
                    CreatedDate = DateTime.Now,
                    Currency = Currency.VND,
                    Language = DisplayLanguage.Vietnamese
                };

                var paymentUrl = _vnpay.GetPaymentUrl(request);

                return Created(paymentUrl, paymentUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Callback")]
        public IActionResult Callback()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnpay.GetPaymentResult(Request.Query);
                    // Append all query parameters to the redirect URL
                    var queryParams = Request.QueryString.Value;


                    if (!paymentResult.IsSuccess)
                    {
                        //return BadRequest("Payment failed.");
                        return Redirect("http://localhost:3000/book/payment-result");
                    }

                    // Kiểm tra xem PaymentId có khớp với invoiceId không
                    var invoice = _unitOfWork.InvoiceRepository.GetByID((int)paymentResult.PaymentId);

                    if (invoice == null)
                    {
                        return NotFound(new { message = "Invoice not found." });
                    }

                    // Cập nhật trạng thái hóa đơn
                    invoice.Status = "Paid";
                    _unitOfWork.InvoiceRepository.Update(invoice);

                    // Lấy danh sách vaccine từ InvoiceDetail
                    //var invoiceDetails = _unitOfWork.InvoiceDetailRepository.Get(d => d.InvoiceId == invoice.InvoiceId);

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

                    //    // Trừ số lượng vaccine trong kho
                    //    vaccineBatch.Quantity -= detail.Quantity;
                    //    _unitOfWork.VaccineBatchDetailRepository.Update(vaccineBatch);
                    //}

                    _unitOfWork.Save();


                    //return Redirect("http://localhost:3000/book/payment-result");
                    return Redirect($"http://localhost:3000/book/payment-result{queryParams}");
                }
                catch (Exception ex)
                {
                    _logger.LogError("Lỗi Callback: {Message}", ex.Message);
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }
    }
}
