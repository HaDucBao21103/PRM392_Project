using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Vaccine.Repo.Entities;

public partial class Vaccine
{
    public int VaccineId { get; set; }

    public string Name { get; set; } = null!;

    public int MaxLateDate { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public int InternalDurationDoses { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    public virtual ICollection<VaccineBatchDetail> VaccineBatchDetails { get; set; } = new List<VaccineBatchDetail>();

    public virtual ICollection<VaccineCombo> Combos { get; set; } = new List<VaccineCombo>();
 
    public virtual ICollection<VaccineComboDetail> VaccineComboDetails { get; set; } = new List<VaccineComboDetail>();
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

}
