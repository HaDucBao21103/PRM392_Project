namespace Vaccine.API.Models.StaffModel
{
    public class RequestUpdateStaffModel
    {
        public string Name { get; set; } = null!;

        public string? Gender { get; set; }

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Role { get; set; }

        public string Password { get; set; } = null!;

        public string? Degree { get; set; }

        public int? ExperienceYears { get; set; }
    }
}
