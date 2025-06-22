using System;
using System.Collections.Generic;

namespace Vaccine.Repo.Entities;

public partial class VaccineComboDetail
{
    public int ComboId { get; set; }

    public int VaccineId { get; set; }
    public virtual VaccineCombo Combo { get; set; } = null!;
    public virtual Vaccine Vaccine { get; set; } = null!;
}
