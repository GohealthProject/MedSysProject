﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class ProductReview
{
    public int ProductReviewId { get; set; }

    public int? MemberId { get; set; }

    public int? Product { get; set; }

    public string ReviewContent { get; set; }

    public DateTime? Timestamp { get; set; }
}