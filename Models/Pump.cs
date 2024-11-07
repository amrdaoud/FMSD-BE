using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class Pump
{
    public Guid Guid { get; set; }

    public long? Id { get; set; }

    public string LogicalAddress { get; set; } = null!;

    public string? PhysicalAddress { get; set; }

    public string PumpFunction { get; set; } = null!;

    public double? TotalDispAmount { get; set; }

    public long? CalibrationLimit { get; set; }

    public long? StatusId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? NozzleStatusId { get; set; }

    public Guid StationGuid { get; set; }

    public virtual ICollection<Calibration> Calibrations { get; set; } = new List<Calibration>();

    public virtual NozzleStatus? NozzleStatus { get; set; }

    public virtual PumpStatus? PumpStatus { get; set; }

    public virtual ICollection<PumpTank> PumpTanks { get; set; } = new List<PumpTank>();

    public virtual ICollection<RfidDevice> RfidDevices { get; set; } = new List<RfidDevice>();

}
