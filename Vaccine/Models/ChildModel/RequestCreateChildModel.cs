using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;
using Vaccine.API.Models.CustomerModel;
using Vaccine.Repo.Entities;

namespace Vaccine.API.Models.ChildModel
{
    public class RequestCreateChildModel
    {
        //public int ChildId { get; set; }

        public int? CustomerId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateOnly Dob { get; set; }

        [RegularExpression("M|F", ErrorMessage = "Status must be 'M' or 'F'")]
        public string? Gender { get; set; }

        public string? BloodType { get; set; }

    }

    /// <summary>
    /// Example request model for Swagger documentation
    /// </summary>
    public class ExampleRequestCreateChildModel : IExamplesProvider<RequestCreateChildModel>
    {
        public RequestCreateChildModel GetExamples()
        {
            return new RequestCreateChildModel
            {
                CustomerId = 1,
                Name = "John Doe",
                Dob = new DateOnly(2000, 1, 1),
                Gender = "M",
                BloodType = "A"
            };
            }
    }
}
