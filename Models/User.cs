using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class User
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Username { get; set; } = null!;

    public long? RoleId { get; set; }

    public DateTime? EmailVerifiedAt { get; set; }

    public string Password { get; set; } = null!;

    public string? RememberToken { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid StationGuid { get; set; }

    public virtual ICollection<Alarm> Alarms { get; set; } = new List<Alarm>();

    public virtual ICollection<Calibration> Calibrations { get; set; } = new List<Calibration>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<RfidCard> RfidCards { get; set; } = new List<RfidCard>();

    public virtual Role? Role { get; set; }

}
