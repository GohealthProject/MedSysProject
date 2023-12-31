﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string ProjectName { get; set; }

    public string ProjectContent { get; set; }

    public double? ProjectPrice { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual ICollection<PlanRef> PlanRefs { get; set; } = new List<PlanRef>();

    public virtual ICollection<SubProjectBridge> SubProjectBridges { get; set; } = new List<SubProjectBridge>();
}