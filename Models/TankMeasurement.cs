using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class TankMeasurement
{
    public long Id { get; set; }

    public double FuelLevel { get; set; }

    public double FuelVolume { get; set; }

    public double Tcv { get; set; }

    public double WaterLevel { get; set; }

    public double WaterVolume { get; set; }

    public double Ullage { get; set; }

    public double Temperature { get; set; }

    public Guid TankGuid { get; set; }

    public long? OldId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Tank Tank { get; set; } = null!;
}
