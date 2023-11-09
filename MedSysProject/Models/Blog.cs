using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class Blog
{
    public int BlogId { get; set; }

    public string Title { get; set; } = null!;

    public int ArticleClassId { get; set; }

    public int Views { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Content { get; set; } = null!;

    public byte[]? BlogImage { get; set; }

    public int EmployeeId { get; set; }

    public virtual BlogCategory ArticleClass { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Employee Employee { get; set; } = null!;
}
