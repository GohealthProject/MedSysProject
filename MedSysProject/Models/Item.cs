using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string? ItemName { get; set; }

    public string? ItemComment { get; set; }

    public int? ItemPrice { get; set; }

    public int? ProjectId { get; set; }

    public string? ItemUnit { get; set; }

    public int? ItemRangeMin { get; set; }

    public int? ItemRangeMax { get; set; }

    public int? Mmin { get; set; }

    public int? Mmax { get; set; }

    public int? Fmin { get; set; }

    public int? Fmax { get; set; }

    public string? ItemRange { get; set; }

    public virtual Project? Project { get; set; }

    public virtual ICollection<ReportDetail> ReportDetails { get; set; } = new List<ReportDetail>();

    public virtual ICollection<ReservedSub> ReservedSubs { get; set; } = new List<ReservedSub>();

    public virtual ICollection<SubProjectBridge> SubProjectBridges { get; set; } = new List<SubProjectBridge>();
}
