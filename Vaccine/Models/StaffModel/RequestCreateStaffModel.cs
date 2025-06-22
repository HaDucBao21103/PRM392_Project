using Vaccine.Repo.Entities;

namespace Vaccine.API.Models.StaffModel
{
    public class RequestCreateStaffModel
    {
        public string Name { get; set; } = null!;

        public DateOnly Dob { get; set; }

        public string? Gender { get; set; }

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Role { get; set; }

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;
        public string? Degree { get; set; }

        public int? ExperienceYears { get; set; }

    }
}
