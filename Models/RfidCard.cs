using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class RfidCard
{
    public long Id { get; set; }

    public long? UserId { get; set; }

    public string? RfidCode { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? LocalId { get; set; }

    public Guid StationGuid { get; set; }

    public virtual User? User { get; set; }
}
