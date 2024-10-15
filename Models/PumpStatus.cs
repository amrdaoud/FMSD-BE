using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class PumpStatus
{
    public long Id { get; set; }

    public string PumpStatus1 { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? LocalId { get; set; }

    public Guid StationGuid { get; set; }

    public virtual ICollection<Pump> Pumps { get; set; } = new List<Pump>();
}
