﻿using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class Reserve
{
    public int ReserveId { get; set; }

    public int? MemberId { get; set; }

    public int? PlanId { get; set; }

    public DateTime? ReserveDate { get; set; }

    public string? ReserveState { get; set; }

    public virtual ICollection<HealthReport> HealthReports { get; set; } = new List<HealthReport>();

    public virtual Member? Member { get; set; }

    public virtual Plan? Plan { get; set; }

    public virtual ICollection<ReservedSub> ReservedSubs { get; set; } = new List<ReservedSub>();
}