using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class Corporation
{
    public int TaxId { get; set; }

    public string Corporation1 { get; set; } = null!;

    public double? Discount { get; set; }

    public int Contactnumber { get; set; }

    public string Address { get; set; } = null!;

    public string? Middleman { get; set; }

    public string Password { get; set; } = null!;

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
}
