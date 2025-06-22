using System.ComponentModel.DataAnnotations;

namespace Vaccine.API.Models.EmailModel
{
    public class RequestSendEmailModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
    }
}
