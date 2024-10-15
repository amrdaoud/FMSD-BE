using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class StationTanksPumVw
{
    public Guid Guid { get; set; }

    public long? Id { get; set; }

    public string StationName { get; set; } = null!;

    public long TankNumber { get; set; }

    public long PumpNumber { get; set; }

    public string City { get; set; } = null!;

    public string TankName { get; set; } = null!;

    public Guid TanksGuid { get; set; }

    public long? TanksId { get; set; }

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

    public Guid PumpsGuid { get; set; }

    public long? PumpsId { get; set; }

    public string PLogicalAddress { get; set; } = null!;

    public string? PPhysicalAddress { get; set; }

    public string PumpFunction { get; set; } = null!;

    public double? TotalDispAmount { get; set; }

    public long? CalibrationLimit { get; set; }

    public long? StatusId { get; set; }

    public string PumpStatus { get; set; } = null!;
}
