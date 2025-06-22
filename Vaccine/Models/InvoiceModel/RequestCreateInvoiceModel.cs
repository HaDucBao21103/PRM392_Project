using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;
using Vaccine.Repo.Entities;

namespace Vaccine.API.Models.InvoiceModel
{
    public class RequestCreateInvoiceModel
    {
        //public int InvoiceId { get; set; }

        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Total Amount is required")]
        public decimal TotalAmount { get; set; }

        //[RegularExpression("Unpaid", ErrorMessage = "Status must be 'Paid' or 'Unpaid'")] 
        //public string? Status { get; set; } = "Unpaid";

        [RegularExpression("Single|Combo", ErrorMessage = "Type must be 'Single' or 'Combo'")]
        public string? Type { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }


    }

    public class RequestCreateInvoiceModel1
    {
        //public int InvoiceId { get; set; }

        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Total Amount is required")]
        public decimal TotalAmount { get; set; }

        //[RegularExpression("Unpaid", ErrorMessage = "Status must be 'Paid' or 'Unpaid'")] 
        //public string? Status { get; set; } = "Unpaid";

        [RegularExpression("Single|Combo", ErrorMessage = "Type must be 'Single' or 'Combo'")]
        public string? Type { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        //public virtual Customer? Customer { get; set; }

        public virtual ICollection<InvoiceDetailDto> InvoiceDetails { get; set; }
    }

    public class InvoiceDto
    {
        public int InvoiceId { get; set; }
        public int? CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
        public DateTime? CreatedAt { get; set; }
        public List<InvoiceDetailDto> InvoiceDetails { get; set; }
    }

    public class InvoiceDetailDto
    {
        public int? VaccineId { get; set; }
        public int? AppointmentId { get; set; }
        public int? ComboId { get; set; }
        public int? Quantity { get; set; }
        public decimal Price { get; set; }
    }

}
