using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Vaccine.Repo.Entities;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int CustomerId { get; set; }

    public int ChildId { get; set; }

    public int? StaffId { get; set; }

    public int? DoctorId { get; set; }

    public string? VaccineType { get; set; }

    public int? VaccineId { get; set; }

    public int? ComboId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string? Status { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? BatchNumber { get; set; }

    public virtual VaccineBatch? BatchNumberNavigation { get; set; }

    public virtual Child Child { get; set; } = null!;

    public virtual VaccineCombo? Combo { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Doctor? Doctor { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    public virtual Staff? Staff { get; set; }
    
    public virtual Vaccine? Vaccine { get; set; }
}
