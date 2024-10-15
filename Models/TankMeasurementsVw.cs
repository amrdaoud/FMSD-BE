using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class TankMeasurementsVw
{
    public double FuelLevel { get; set; }

    public long Id { get; set; }

    public double Tcv { get; set; }

    public double FuelVolume { get; set; }

    public double WaterLevel { get; set; }

    public double WaterVolume { get; set; }

    public double Ullage { get; set; }

    public double Temperature { get; set; }

    public Guid TankGuid { get; set; }

    public long? OldId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string TankName { get; set; } = null!;

    public string City { get; set; } = null!;

    public string StationName { get; set; } = null!;

    public Guid StationGuid { get; set; }

    public double DLLimit { get; set; }

    public double WLLimit { get; set; }

    public double MLLimit { get; set; }

    public double LowLimit { get; set; }

    public double LowLowLimit { get; set; }

    public double HighLimit { get; set; }

    public double HighHighLimit { get; set; }

    public double Hysteresis { get; set; }

    public double WaterHighLimit { get; set; }

    public double LogicalAddress { get; set; }

    public int PhysicalAddress { get; set; }

    public double Height { get; set; }

    public string? Description { get; set; }

    public double Capacity { get; set; }
}
