using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public string? MemberName { get; set; }

    public string? MemberGender { get; set; }

    public DateTime? MemberBirthdate { get; set; }

    public string MemberPhone { get; set; } = null!;

    public string MemberEmail { get; set; } = null!;

    public string? MemberAddress { get; set; }

    public string? MemberContactNumber { get; set; }

    public string? MemberNickname { get; set; }

    public string? MemberPassword { get; set; }

    public int? TaxId { get; set; }

    public string? MemberAccount { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<HealthReport> HealthReports { get; set; } = new List<HealthReport>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Reserve> Reserves { get; set; } = new List<Reserve>();

    public virtual Corporation? Tax { get; set; }
}
