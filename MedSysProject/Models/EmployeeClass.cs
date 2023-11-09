using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class EmployeeClass
{
    public int EmployeeClassId { get; set; }

    public string Class { get; set; } = null!;

    public int BlogPermissionId { get; set; }

    public virtual BlogManagement BlogPermission { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
