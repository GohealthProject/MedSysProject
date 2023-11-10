﻿using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class SubProjectBridge
{
    public int SubProjectBridgeId { get; set; }

    public int? ProjectId { get; set; }

    public int? ItemId { get; set; }

    public virtual Item? Item { get; set; }

    public virtual Project? Project { get; set; }
}