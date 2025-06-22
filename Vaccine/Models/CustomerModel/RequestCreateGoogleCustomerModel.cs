using System.ComponentModel.DataAnnotations;

namespace Vaccine.API.Models.CustomerModel
{
    public class RequestCreateGoogleCustomerModel
    {
        public string Name { get; set; } = null!;

        [Required]
        public string? Email { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
