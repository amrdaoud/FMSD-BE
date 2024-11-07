using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FMSD_BE.Models;

public partial class FuelTransaction
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


    public Guid? TankGuid { get; set; }

    public long? UserId { get; set; }

    public double? OrderedAmount { get; set; }

    public long OperationTypeId { get; set; }

    public string? RequisitionNumber { get; set; }

    public string? Mode { get; set; }

    public Guid? RequestedTankGuid { get; set; }

    public long? OldId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public double LiterPrice { get; set; }

    public string? Note { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid StationGuid { get; set; }

    public virtual OperationType OperationType { get; set; } = null!;
    public virtual TransactionStatus? TransactionStatus { get; set; }



}
