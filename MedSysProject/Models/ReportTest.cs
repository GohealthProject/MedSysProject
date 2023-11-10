﻿using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class ReportTest
{
    public int ReportDetailId { get; set; }

    public int ReportId { get; set; }

    public int? ItemId { get; set; }

    public string? Result { get; set; }

    public virtual Item? Item { get; set; }

    public virtual HealthReport Report { get; set; } = null!;
}