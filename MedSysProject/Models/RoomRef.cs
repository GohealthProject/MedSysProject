﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace MedSysProject.Models;

public partial class RoomRef
{
    public int RoombridgeId { get; set; }

    public int Roomid { get; set; }

    public int? Memberid { get; set; }

    public int? Employeeid { get; set; }

    public virtual Employee Employee { get; set; }

    public virtual Member Member { get; set; }

    public virtual Room Room { get; set; }
}