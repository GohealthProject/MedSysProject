﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class ReturnProduct
{
    public int ReturnId { get; set; }

    public int? OrderId { get; set; }

    public string ReturnState { get; set; }

    public DateTime? ReturnDate { get; set; }

    public DateTime? ProcessedDate { get; set; }

    public decimal? RefundAmount { get; set; }
}