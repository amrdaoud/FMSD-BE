using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class Tank
{
    public Guid Guid { get; set; }

    public long? Id { get; set; }

    public string TankName { get; set; } = null!;

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

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? TankStatusId { get; set; }

    public virtual ICollection<CalibrationDetail> CalibrationDetails { get; set; } = new List<CalibrationDetail>();

    public virtual ICollection<Leakage> Leakages { get; set; } = new List<Leakage>();

    public virtual ICollection<PumpTank> PumpTanks { get; set; } = new List<PumpTank>();

    public virtual Station Station { get; set; } = null!;

    public virtual ICollection<TankMeasurement> TankMeasurements { get; set; } = new List<TankMeasurement>();

    public virtual TankStatus? TankStatus { get; set; }

    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
}
