namespace Vaccine.API.Controllers
{
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;
    using System.Net.Mail;
    using Vaccine.API.Helper;
    using Vaccine.API.Models.EmailModel;
    using Vaccine.API.Models.HolidayModel;
    using Vaccine.Repo.Entities;
    using Vaccine.Repo.Repository;
    using Vaccine.Repo.UnitOfWork;

    [EnableCors("MyPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {

        [HttpPost("sendEmail")]
        public IActionResult SendEmail(RequestSendEmailModel requestSendEmailModel)
        {
            // check email có đúng cú pháp không
            if (!Utils.IsValidEmail(requestSendEmailModel.Email)) {
                return BadRequest("Email không hợp lệ");
            }

            EmailRepository.SendEmail(requestSendEmailModel.Email, requestSendEmailModel.Subject, requestSendEmailModel.Body);

            return Ok();
        }

    }
}
