using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class OrderState
{
    public int StateId { get; set; }

    public string? StateName { get; set; }

    public string? StateDetailed { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
