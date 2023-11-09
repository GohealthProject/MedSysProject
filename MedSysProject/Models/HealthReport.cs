using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class HealthReport
{
    public int ReportId { get; set; }

    public int? MemberId { get; set; }

    public DateTime? ReportDate { get; set; }

    public int? PlanId { get; set; }

    public int? ReserveId { get; set; }

    public int? Paymentstatus { get; set; }

    public virtual Member? Member { get; set; }

    public virtual Plan? Plan { get; set; }

    public virtual ICollection<ReportDetail> ReportDetails { get; set; } = new List<ReportDetail>();

    public virtual Reserve? Reserve { get; set; }
}
