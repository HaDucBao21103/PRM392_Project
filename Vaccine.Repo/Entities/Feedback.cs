using System;
using System.Collections.Generic;

namespace Vaccine.Repo.Entities;

public partial class Feedback
{
    public int ReviewId { get; set; }

    public int CustomerId { get; set; }

    public int? DoctorId { get; set; }

    public int? StaffId { get; set; }

    public int? VaccineId { get; set; }

    public int AppointmentId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Doctor? Doctor { get; set; }
}
