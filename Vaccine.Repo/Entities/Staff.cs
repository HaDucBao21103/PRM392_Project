using System;
using System.Collections.Generic;

namespace Vaccine.Repo.Entities;

public partial class Staff
{
    public int StaffId { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public string? Gender { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Role { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Status { get; set; }

    public string? Degree { get; set; }

    public int? ExperienceYears { get; set; }
    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
