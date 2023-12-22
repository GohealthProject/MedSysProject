﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class Member
{
    public int MemberId { get; set; }

    public string MemberName { get; set; }

    public string MemberGender { get; set; }

    public DateTime? MemberBirthdate { get; set; }

    public string MemberPhone { get; set; }

    public string MemberEmail { get; set; }

    public int? MemberCityDistrictRoadId { get; set; }

    public string MemberAddress { get; set; }

    public string MemberContactNumber { get; set; }

    public string MemberNickname { get; set; }

    public string MemberPassword { get; set; }

    public int? TaxId { get; set; }

    public string MemberAccount { get; set; }

    public string MemberImage { get; set; }

    public bool IsVerified { get; set; }

    public int? StatusId { get; set; }

    public string VieifiedId { get; set; }

    public bool? GoogleLogin { get; set; }

    public bool? FacebookLogin { get; set; }

    public bool? LineLogin { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<HealthReport> HealthReports { get; set; } = new List<HealthReport>();

    public virtual Twaddress MemberCityDistrictRoad { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();

    public virtual ICollection<Reserve> Reserves { get; set; } = new List<Reserve>();

    public virtual ICollection<RoomRef> RoomRefs { get; set; } = new List<RoomRef>();

    public virtual MembersStatus Status { get; set; }

    public virtual Corporation Tax { get; set; }

    public virtual ICollection<TrackingList> TrackingLists { get; set; } = new List<TrackingList>();
}