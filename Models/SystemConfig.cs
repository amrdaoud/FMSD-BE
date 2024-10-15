using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class SystemConfig
{
    public long Id { get; set; }

    public int? RefreshTime { get; set; }

    public double LiterPrice { get; set; }

    public Guid StationGuid { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Station Station { get; set; } = null!;
}
