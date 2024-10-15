using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class Permission
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Group { get; set; } = null!;

    public string GuardName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long LocalId { get; set; }

    public Guid StationGuid { get; set; }
}
