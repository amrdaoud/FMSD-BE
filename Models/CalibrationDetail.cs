using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class CalibrationDetail
{
    public long Id { get; set; }

    public long CalibrationId { get; set; }

    public Guid TankGuid { get; set; }

    public double FuelAfter { get; set; }

    public double FuelBefore { get; set; }

    public double TcvAfter { get; set; }

    public double TcvBefore { get; set; }

    public long? OldId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid StationGuid { get; set; }

    public virtual Tank Tank { get; set; } = null!;
}
