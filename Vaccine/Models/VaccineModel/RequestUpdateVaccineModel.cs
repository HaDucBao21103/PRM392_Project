using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;

namespace Vaccine.API.Models.VaccineModel
{
    public class RequestUpdateVaccineModel
    {

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "MaxLateDate must be a positive integer")]
        public int MaxLateDate { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        public int InternalDurationDoses { get; set; }
    }
    public class ExampleRequestUpdateVaccineModel : IExamplesProvider<RequestUpdateVaccineModel>
    {
        public RequestUpdateVaccineModel GetExamples()
        {
            return new RequestUpdateVaccineModel
            {
                Name = "Updated COVID-19 Vaccine",
                MaxLateDate = 45,
                Price = 160,
                Description = "Updated description for COVID-19 vaccine",
                InternalDurationDoses = 2
            };
        }
    }

}
