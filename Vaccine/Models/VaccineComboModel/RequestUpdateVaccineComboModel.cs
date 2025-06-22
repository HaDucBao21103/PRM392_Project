using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;

namespace Vaccine.API.Models.VaccineComboModel
{
    public class RequestUpdateVaccineComboModel 
    {
        //[Required(ErrorMessage = "ComboId is required for updating")]
        //public int ComboId { get; set; } // Bắt buộc khi cập nhật
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "VaccineList cannot be empty")]
        [MinLength(1, ErrorMessage = "VaccineList must have one or more Vaccine")]
        public List<int> VaccineIds { get; set; }
    }
    public class ExampleRequestUpdateVaccineComboModel : IExamplesProvider<RequestUpdateVaccineComboModel>
    {
        public RequestUpdateVaccineComboModel GetExamples()
        {
            return new RequestUpdateVaccineComboModel
            {
                
                Name = "Test vaccine combo update",
                Description = "Tesst vaccine des",
                Price = 150,
                VaccineIds = new List<int> { 1, 2, 3 }
            };
        }
    }
}
