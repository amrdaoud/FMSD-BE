using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class Notification
{
    public long Id { get; set; }

    public Guid StationGuid { get; set; }

    public long? NotificationTypeId { get; set; }

    public string? Show { get; set; }

    public long? AcknowledgedUserId { get; set; }

    public long? OldId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Description { get; set; }

    public virtual NotificationType? NotificationType { get; set; }

    public virtual User? User { get; set; }
}
