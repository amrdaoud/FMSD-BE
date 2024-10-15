using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class Alarm
{
    public long Id { get; set; }

    public string AlarmCode { get; set; } = null!;

    public string Description { get; set; } = null!;

    public long? AcknowledgedUserId { get; set; }

    public DateTime? AcknowledgedTime { get; set; }

    public bool? Show { get; set; }

    public string? Type { get; set; }

    public long AlarmStatusId { get; set; }

    public Guid StationGuid { get; set; }

    public DateTime? FixTime { get; set; }

    public long? OldId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? TrrigerId { get; set; }

    public virtual AlarmStatus AlarmStatus { get; set; } = null!;

    public virtual Station Station { get; set; } = null!;

    public virtual User? User { get; set; }
}
