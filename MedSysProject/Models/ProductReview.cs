﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class ProductReview
{
    public int ProductReviewId { get; set; }

    public int? MemberId { get; set; }

    public int? ProductId { get; set; }

    public string ReviewContent { get; set; }

    public DateTime? Timestamp { get; set; }

    public bool? IsLike { get; set; }

    public virtual Member Member { get; set; }

    public virtual Product Product { get; set; }
}