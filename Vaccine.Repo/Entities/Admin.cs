using System;
using System.Collections.Generic;

namespace Vaccine.Repo.Entities;

public partial class Admin
{
    public int AdminId { get; set; }

    public string UserName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public string? Gender { get; set; }

    public string? BloodType { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<Holiday> Holidays { get; set; } = new List<Holiday>();
}
