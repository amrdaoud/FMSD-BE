using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class TankStatus
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid StationGuid { get; set; }

    public long LocalId { get; set; }

    public virtual ICollection<Tank> Tanks { get; set; } = new List<Tank>();
}
