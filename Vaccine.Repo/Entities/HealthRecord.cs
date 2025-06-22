using System;
using System.Collections.Generic;

namespace Vaccine.Repo.Entities;

public partial class HealthRecord
{
    public int RecordId { get; set; }

    public int? StaffId { get; set; }

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

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Doctor? Doctor { get; set; }

    public virtual Staff? Staff { get; set; }
}
