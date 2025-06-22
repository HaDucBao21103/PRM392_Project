using System;
using System.Collections.Generic;

namespace Vaccine.Repo.Entities;

public partial class Child
{
    public int ChildId { get; set; }

    public int CustomerId { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public string? Gender { get; set; }

    public string? BloodType { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Customer Customer { get; set; } = null!;
}
