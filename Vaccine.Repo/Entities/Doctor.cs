using System;
using System.Collections.Generic;

namespace Vaccine.Repo.Entities;

public partial class Doctor
{
    public int DoctorId { get; set; }

    public string Name { get; set; } = null!;

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public string Phone { get; set; } = null!;

    public string? Address { get; set; }

    public string? Degree { get; set; }

    public int? ExperienceYears { get; set; }

    public DateOnly Dob { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();
}
