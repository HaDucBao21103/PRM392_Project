using System;
using System.Collections.Generic;

namespace Vaccine.Repo.Entities;

public partial class VaccineBatchDetail
{
    public string BatchNumber { get; set; } = null!;

    public int VaccineId { get; set; }

    public int? Quantity { get; set; }
    public int PreOrderQuantity { get; set; }
    public virtual VaccineBatch BatchNumberNavigation { get; set; } = null!;

    public virtual Vaccine Vaccine { get; set; } = null!;
}
