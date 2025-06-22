namespace Vaccine.API.Models.DoctorModel
{
    public class RequestCreateDoctorModel
    {
        public string Name { get; set; } = null!;
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string Phone { get; set; } = null!;
        public string? Address { get; set; }
        public string? Degree { get; set; }
        public int? ExperienceYears { get; set; }
        public DateOnly Dob { get; set; }
    }
}
