using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class Leakage
{
    public long Id { get; set; }

    public Guid TankGuid { get; set; }

    public double? Deviation { get; set; }

    public double? Limit { get; set; }

    public string Leakage1 { get; set; } = null!;

    public long? OldId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? LeakageType { get; set; }

    public virtual Tank Tank { get; set; } = null!;
}
