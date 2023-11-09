using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class ReservedSub
{
    public int SubreservedId { get; set; }

    public int ReservedId { get; set; }

    public int ItemId { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Reserve Reserved { get; set; } = null!;
}
