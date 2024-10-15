using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class PumpTank
{
    public long Id { get; set; }

    public Guid PumpGuid { get; set; }

    public Guid TankGuid { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Pump Pump { get; set; } = null!;

    public virtual Tank Tank { get; set; } = null!;
}
