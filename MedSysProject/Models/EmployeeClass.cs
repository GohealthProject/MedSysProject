﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class EmployeeClass
{
    public int EmployeeClassId { get; set; }

    public string Class { get; set; }

    public int BlogPermissionId { get; set; }

    public virtual BlogManagement BlogPermission { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}