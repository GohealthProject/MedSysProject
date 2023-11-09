using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class PlanRef
{
    public int PlanbridgeId { get; set; }

    public int PlanId { get; set; }

    public int? ProjectId { get; set; }

    public virtual Plan Plan { get; set; } = null!;

    public virtual Project? Project { get; set; }
}
