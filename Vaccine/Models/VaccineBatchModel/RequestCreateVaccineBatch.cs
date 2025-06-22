using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vaccine.API.Models.VaccineBatchModel
{
    public class RequestCreateVaccineBatch
    {

            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.None)] 
            [Required]
            [StringLength(50, ErrorMessage = "Batch number cannot exceed 50 characters.")]
            public string BatchNumber { get; set; }

            [Required]
            [StringLength(255, ErrorMessage = "Manufacturer name cannot exceed 255 characters.")]
            public string Manufacturer { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateOnly ManufactureDate { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateOnly ExpiryDate { get; set; }

            [Required]
            [StringLength(255, ErrorMessage = "Country name cannot exceed 255 characters.")]
            public string Country { get; set; }

            //[Range(1, 60, ErrorMessage = "Duration must be between 1 and 60 months.")]
            //public int? Duration { get; set; }

            [Required]
            [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
            public string Status { get; set; }

            [Required(ErrorMessage = "Vaccine batch must include at least one vaccine.")]
            public List<RequestCreateVaccineBatchDetail> VaccineBatchDetails { get; set; }
        

        public class RequestCreateVaccineBatchDetail
        {
            [Required]
            public int VaccineId { get; set; }

            [Required]
            [Range(1, 10000, ErrorMessage = "Quantity must be between 1 and 10,000 doses.")]
            public int Quantity { get; set; }
        }
    }
}
