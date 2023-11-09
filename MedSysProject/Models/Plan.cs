using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class Plan
{
    public int PlanId { get; set; }

    public string PlanName { get; set; } = null!;

    public string? PlanDescription { get; set; }

    public virtual ICollection<HealthReport> HealthReports { get; set; } = new List<HealthReport>();

    public virtual ICollection<PlanRef> PlanRefs { get; set; } = new List<PlanRef>();

    public virtual ICollection<Reserve> Reserves { get; set; } = new List<Reserve>();
}
