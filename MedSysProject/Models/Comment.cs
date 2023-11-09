﻿using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public int BlogId { get; set; }

    public int? MemberId { get; set; }

    public int? EmployeeId { get; set; }

    public int? ParentCommentId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Blog Blog { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Comment> InverseParentComment { get; set; } = new List<Comment>();

    public virtual Member? Member { get; set; }

    public virtual Comment? ParentComment { get; set; }
}
