using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class Role
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? LocalId { get; set; }

    public Guid StationGuid { get; set; }

    public string? GuardName { get; set; }

    public virtual ICollection<NotificationTypeRole> NotificationTypeRoles { get; set; } = new List<NotificationTypeRole>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
