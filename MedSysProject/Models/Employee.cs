using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public int EmployeeClassId { get; set; }

    public DateTime? EmployeeBirthDate { get; set; }

    public string EmployeePhoneNum { get; set; } = null!;

    public string EmployeeEmail { get; set; } = null!;

    public string EmployeePassWord { get; set; } = null!;

    public byte[]? EmployeePhoto { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual EmployeeClass EmployeeClass { get; set; } = null!;
}
