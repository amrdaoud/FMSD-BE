using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class ModelHasRole
{
    public long RoleId { get; set; }

    public string ModelType { get; set; } = null!;

    public long? ModelId { get; set; }

    public Guid? StationGuid { get; set; }

    public virtual Role? Role { get; set; }
}
