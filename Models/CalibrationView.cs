using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class CalibrationView
{
    public long Id { get; set; }

    public Guid PumpGuid { get; set; }

    public double MeasuredAmount { get; set; }

    public double OrderedAmount { get; set; }

    public double? DispAmountPump { get; set; }

    public long OldId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long UserId { get; set; }

    public string LogicalAddress { get; set; } = null!;

    public string? PhysicalAddress { get; set; }

    public string StationName { get; set; } = null!;

    public string City { get; set; } = null!;

    public long? CalibrationLimit { get; set; }

    public double? TotalDispAmount { get; set; }

    public string? Username { get; set; }

    public string? Name { get; set; }

    public long? RowNumber { get; set; }
}
