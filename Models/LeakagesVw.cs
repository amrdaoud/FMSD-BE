using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class LeakagesVw
{
    public long Id { get; set; }

    public Guid TankGuid { get; set; }

    public double? Deviation { get; set; }

    public double? Limit { get; set; }

    public string Leakage { get; set; } = null!;

    public long? OldId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string TankName { get; set; } = null!;

    public string StationName { get; set; } = null!;

    public double Capacity { get; set; }

    public string City { get; set; } = null!;

    public int? Week { get; set; }

    public int? Month { get; set; }

    public int? Year { get; set; }

    public int? WeekOfMonth { get; set; }

    public string Type { get; set; } = null!;

    public long? RowNumber { get; set; }
}
