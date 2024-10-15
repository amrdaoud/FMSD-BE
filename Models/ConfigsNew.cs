using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class ConfigsNew
{
    public long Id { get; set; }

    public string Type { get; set; } = null!;

    public string Key { get; set; } = null!;

    public string? Value { get; set; }

    public string? Info { get; set; }

    public Guid? StationGuid { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long LocalId { get; set; }

    public virtual Station? Station { get; set; }
}
