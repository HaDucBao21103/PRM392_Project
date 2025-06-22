using Swashbuckle.AspNetCore.Filters;
using Vaccine.Repo.Entities;

namespace Vaccine.API.Models.CustomerModel
{
    public class RequestCreateCustomerModel
    {
        public string Name { get; set; } = null!;

        public DateOnly Dob { get; set; }

        public string? Gender { get; set; }

        public string Phone { get; set; } = null!;

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? BloodType { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

    }

    /// <summary>
    /// Example request model for Swagger documentation
    /// </summary>
    public class ExampleCreateCustomerModel : IExamplesProvider<RequestCreateCustomerModel>
    {
        public RequestCreateCustomerModel GetExamples()
        {
            return new RequestCreateCustomerModel
            {
                Name = "John Doe",
                Dob = new DateOnly(1995, 5, 20), // Ngày sinh giả định
                Gender = "M",
                Phone = "0987654321",
                Email = "johndoe@gmail.com",
                Address = "123 Main Street, New York",
                BloodType = "O",
                UserName = "johndoe95",
                Password = "SecurePass123"
            };
        }
    }


}
