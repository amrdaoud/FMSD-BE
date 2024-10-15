using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class FuelTransactionsVwUpdated
{
    public long Id { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public double? DispensedAmount { get; set; }

    public double? MeasuredAmount { get; set; }

    public long? TransactionStatusId { get; set; }

    public string? DispensedTankGuid { get; set; }

    public string? DriverName { get; set; }

    public string? TankerPlate { get; set; }

    public string? DriverLicense { get; set; }

    public string? AccompanyingName { get; set; }

    public Guid? PumpGuid { get; set; }

    public Guid? RequestedTankGuid { get; set; }

    public long? UserId { get; set; }

    public string Username { get; set; } = null!;

    public double? OrderedAmount { get; set; }

    public long OperationTypeId { get; set; }

    public string? RequisitionNumber { get; set; }

    public string? Mode { get; set; }

    public long? OldId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public double LiterPrice { get; set; }

    public string TransStatus { get; set; } = null!;

    public long? FuelTransactionId { get; set; }

    public Guid? TankGuid { get; set; }

    public double? FuelVolumeAfter { get; set; }

    public double? FuelVolumeBefore { get; set; }

    public double? TcvAfter { get; set; }

    public double? TcvBefore { get; set; }

    public long? DetailsOldId { get; set; }

    public string Type { get; set; } = null!;

    public string LogicalAddress { get; set; } = null!;

    public string? PhysicalAddress { get; set; }

    public string? DispensedTankName { get; set; }

    public double? DispensedTankCapacity { get; set; }

    public string StationName { get; set; } = null!;

    public string StationCity { get; set; } = null!;

    public string? DetailTankName { get; set; }

    public long? RowNumber { get; set; }
}
