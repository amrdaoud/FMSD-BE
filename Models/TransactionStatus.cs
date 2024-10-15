using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class TransactionStatus
{
    public long Id { get; set; }

    public string TransStatus { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? LocalId { get; set; }

    public Guid StationGuid { get; set; }

    public virtual ICollection<FuelTransaction> FuelTransactions { get; set; } = new List<FuelTransaction>();
}
