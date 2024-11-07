using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class Station
{
    public Guid Guid { get; set; }

    public long? Id { get; set; }

    public string StationName { get; set; } = null!;

    public long TankNumber { get; set; }

    public long PumpNumber { get; set; }

    public string City { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? StationType { get; set; }

    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

    public virtual ICollection<Alarm> Alarms { get; set; } = new List<Alarm>();

    public virtual ICollection<ConfigsNew> ConfigsNews { get; set; } = new List<ConfigsNew>();

    public virtual ICollection<EngineerConfig> EngineerConfigs { get; set; } = new List<EngineerConfig>();

    public virtual ICollection<MyLog> MyLogs { get; set; } = new List<MyLog>();

    public virtual ICollection<NotificationsNew> NotificationsNews { get; set; } = new List<NotificationsNew>();

    public virtual ICollection<SystemConfig> SystemConfigs { get; set; } = new List<SystemConfig>();

    public virtual ICollection<Tank> Tanks { get; set; } = new List<Tank>();
    public virtual ICollection<FuelTransaction> FuelTransactions { get; set; } = new List<FuelTransaction>();

}
