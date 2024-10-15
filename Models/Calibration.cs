using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class Calibration
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

    public string? Note { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid StationGuid { get; set; }

    public virtual Pump Pump { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
