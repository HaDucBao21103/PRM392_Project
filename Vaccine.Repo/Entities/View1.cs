using System;
using System.Collections.Generic;

namespace Vaccine.Repo.Entities;

public partial class View1
{
    public int InvoiceId { get; set; }

    public int CustomerId { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Status { get; set; }

    public string? Type { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int DetailId { get; set; }

    public int Expr1 { get; set; }

    public int? VaccineId { get; set; }

    public int? AppointmentId { get; set; }

    public int? ComboId { get; set; }

    public int? Quantity { get; set; }

    public decimal Price { get; set; }
}
