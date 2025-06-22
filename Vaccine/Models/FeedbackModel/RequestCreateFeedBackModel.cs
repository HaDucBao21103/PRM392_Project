namespace Vaccine.API.Models.FeedbackModel
{
    public class RequestCreateFeedBackModel
    {
        public int CustomerId { get; set; }

        public int? DoctorId { get; set; }

        public int? StaffId { get; set; }

        public int? VaccineId { get; set; }

        public int AppointmentId { get; set; }

        public int? Rating { get; set; }

        public string? Comment { get; set; }
    }
}
