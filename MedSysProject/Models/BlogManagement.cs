using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class BlogManagement
{
    public int BlogPermissionId { get; set; }

    public string PermissionType { get; set; } = null!;

    public virtual ICollection<EmployeeClass> EmployeeClasses { get; set; } = new List<EmployeeClass>();
}
