using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Vaccine.Repo.Entities;

public partial class VaccineCombo
{
    public int ComboId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();

    public virtual ICollection<Vaccine> Vaccines { get; set; } = new List<Vaccine>();

    public virtual ICollection<VaccineComboDetail> VaccineComboDetails { get; set; } = new List<VaccineComboDetail>();

}
