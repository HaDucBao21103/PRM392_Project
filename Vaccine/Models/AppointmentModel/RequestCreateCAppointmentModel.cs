using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;
using Vaccine.Repo.Entities;

namespace Vaccine.API.Models.CustomerModel
{
    public class RequestCreateAppointmentModel
    {
        //public int AppointmentId { get; set; }
        public int CustomerId { get; set; }

        public int ChildId { get; set; }

        //public int? StaffId { get; set; }

        //public int? DoctorId { get; set; }

        //[RegularExpression("Single|Combo", ErrorMessage = "VaccineType must be 'Single' or 'Combo'")]
        //public string VaccineType { get; set; }

        public int VaccineId { get; set; }

       // public int ComboId { get; set; }

        [Required(ErrorMessage = "AppointmentDate is required")]
        public DateTime AppointmentDate { get; set; }

        //[RegularExpression("Pending|Approved|Rejected|Late|Success", ErrorMessage = "Status must be a valid option")]
        //public string? Status { get; set; } = "Pending";

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
