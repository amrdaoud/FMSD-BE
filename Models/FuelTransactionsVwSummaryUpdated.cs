using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class FuelTransactionsVwSummaryUpdated
{
    public long CentralizedDbId { get; set; }

    public long? TransactionId { get; set; }

    public long TransactionTypeId { get; set; }

    public string TransactionType { get; set; } = null!;

    public long? StatusId { get; set; }

    public string Status { get; set; } = null!;

    public double FuelAmount { get; set; }

    public string? TankerPlateNumber { get; set; }

    public string? DriverName { get; set; }

    public long? StationsId { get; set; }

    public string StationName { get; set; } = null!;

    public string StationCity { get; set; } = null!;

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public long? RowNumber { get; set; }
}
