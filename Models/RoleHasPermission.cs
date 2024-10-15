using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class RoleHasPermission
{
    public long PermissionId { get; set; }

    public long RoleId { get; set; }

    public Guid? StationGuid { get; set; }

    public virtual Permission? Permission { get; set; }

    public virtual Role? Role { get; set; }
}
