using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class OrderShip
{
    public int ShipId { get; set; }

    public string? ShipName { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
