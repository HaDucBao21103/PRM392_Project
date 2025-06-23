using System;
using System.Collections.Generic;

namespace Vaccine.Repo.Entities;

public partial class VaccineBatch
{
    public string BatchNumber { get; set; } = null!;

    public string? Manufacturer { get; set; }

    public DateOnly ManufactureDate { get; set; }

    public DateOnly ExpiryDate { get; set; }

    public string? Country { get; set; }

    public int? Duration { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<VaccineBatchDetail> VaccineBatchDetails { get; set; } = new List<VaccineBatchDetail>();
}
