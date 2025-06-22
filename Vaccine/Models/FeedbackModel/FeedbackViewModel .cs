using Vaccine.Repo.Entities;

namespace Vaccine.API.Models.FeedbackModel
{
    public class FeedbackViewModel
    {
        public int ReviewId { get; set; }
        public string? CustomerName { get; set; }

        public string? StaffName { get; set; }
        public string? DoctorName { get; set; }
        public int AppointmentId { get; set; }
        public string? AppointmentDate { get; set; }
        public string? AppointmentStatus { get; set; }
        public int? Rating { get; set; }

        public string? Comment { get; set; }

        public string? CreatedAt { get; set; }

        public string? UpdatedAt { get; set; }
        public string? VaccineName { get; set; }

        public FeedbackViewModel(Feedback feedback)
        {
            ReviewId = feedback.ReviewId;
            CustomerName = feedback.Customer?.Name;
            StaffName = feedback.Staff?.Name;
            VaccineName = feedback.Vaccine?.Name;
            DoctorName = feedback.Doctor?.Name;
            AppointmentId = feedback.AppointmentId;
            AppointmentDate = feedback.Appointment.AppointmentDate.ToString("dd/MM/yyyy");
            AppointmentStatus = feedback.Appointment?.Status;
            Rating = feedback.Rating;
            Comment = feedback.Comment;
            CreatedAt = feedback.CreatedAt?.ToString("dd/MM/yyyy");
            UpdatedAt = feedback.UpdatedAt?.ToString("dd/MM/yyyy");
        }

    }
}
