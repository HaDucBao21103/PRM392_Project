using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;
using Vaccine.Repo.Entities;

namespace Vaccine.API.Models
{
    public class RequestCreateHealthRecordsModel
    {
        public int? StaffId { get; set; }
        [Required]
        public int AppointmentId { get; set; }
        public int? DoctorId { get; set; }
        public string? BloodPressure { get; set; }
        public string? HeartRate { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Temperature { get; set; }
        public bool? AteBeforeVaccine { get; set; }
        public bool? ConditionOk { get; set; }
        public string? ReactionNotes { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
