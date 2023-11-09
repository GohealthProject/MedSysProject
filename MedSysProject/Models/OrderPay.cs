using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class OrderPay
{
    public int PayId { get; set; }

    public string? PayName { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
