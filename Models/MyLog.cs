using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class MyLog
{
    public long Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Setting { get; set; } = null!;

    public string Before { get; set; } = null!;

    public string After { get; set; } = null!;

    public Guid StationGuid { get; set; }

    public long? OldId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Station Station { get; set; } = null!;
}
