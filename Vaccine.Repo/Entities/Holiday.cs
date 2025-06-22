using System;
using System.Collections.Generic;

namespace Vaccine.Repo.Entities;

public partial class Holiday
{
    public int HolidayId { get; set; }

    public int? AdminId { get; set; }

    public DateOnly DateFrom { get; set; }

    public DateOnly DateTo { get; set; }

    public string? Reason { get; set; }

    public virtual Admin? Admin { get; set; }
}
