using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class EngineerConfig
{
    public long Id { get; set; }

    public double PumpOutRate { get; set; }

    public double PumpDurRate { get; set; }

    public string ComAtg { get; set; } = null!;

    public string ComDesp { get; set; } = null!;

    public double SenOutRate { get; set; }

    public double SenDurRate { get; set; }

    public string AtgProtocol { get; set; } = null!;

    public string ComRfid { get; set; } = null!;

    public Guid StationGuid { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Station Station { get; set; } = null!;
}
