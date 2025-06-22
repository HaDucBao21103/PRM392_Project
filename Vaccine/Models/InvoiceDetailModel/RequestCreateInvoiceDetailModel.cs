using Vaccine.Repo.Entities;

namespace Vaccine.API.Models.InvoiceDetailModel
{
    public class RequestCreateInvoiceDetailModel
    {

        public int InvoiceId { get; set; }

        public int? VaccineId { get; set; }

        public int? AppointmentId { get; set; }

        public int? ComboId { get; set; }

        public int? Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
