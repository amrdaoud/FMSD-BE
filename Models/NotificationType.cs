using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class NotificationType
{
    public long Id { get; set; }

    public string Type { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? LocalId { get; set; }

    public Guid StationGuid { get; set; }

    public virtual ICollection<NotificationTypeRole> NotificationTypeRoles { get; set; } = new List<NotificationTypeRole>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
