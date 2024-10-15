using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class ModelHasPermission
{
    public long PermissionId { get; set; }

    public string ModelType { get; set; } = null!;

    public long ModelId { get; set; }

    public Guid? StationGuid { get; set; }

    public virtual Permission? Permission { get; set; }
}
