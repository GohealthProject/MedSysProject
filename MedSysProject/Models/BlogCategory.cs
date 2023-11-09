using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class BlogCategory
{
    public int BlogClassId { get; set; }

    public string BlogCategory1 { get; set; } = null!;

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}
