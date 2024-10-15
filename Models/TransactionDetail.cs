using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class TransactionDetail
{
    public long Id { get; set; }

    public long? FuelTransactionId { get; set; }

    public Guid? TankGuid { get; set; }

    public double? FuelVolumeAfter { get; set; }

    public double? FuelVolumeBefore { get; set; }

    public double? TcvAfter { get; set; }

    public double? TcvBefore { get; set; }

    public long? OldId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid StationGuid { get; set; }

    public virtual Tank? Tank { get; set; }
}
