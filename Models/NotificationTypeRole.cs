using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class NotificationTypeRole
{
    public long Id { get; set; }

    public long NotificationTypeId { get; set; }

    public long RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? LocalId { get; set; }

    public Guid? StationGuid { get; set; }

    public virtual NotificationType? NotificationType { get; set; }

    public virtual Role? Role { get; set; }
}
