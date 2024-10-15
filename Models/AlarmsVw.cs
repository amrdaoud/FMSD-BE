using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class AlarmsVw
{
    public string Status { get; set; } = null!;

    public long Id { get; set; }

    public string AlarmCode { get; set; } = null!;

    public string Description { get; set; } = null!;

    public long? AcknowledgedUserId { get; set; }

    public string? Name { get; set; }

    public DateTime? AcknowledgedTime { get; set; }

    public DateTime? FixTime { get; set; }

    public bool? Show { get; set; }

    public string? Type { get; set; }

    public long AlarmStatusId { get; set; }

    public Guid StationGuid { get; set; }

    public long? OldId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string StationName { get; set; } = null!;

    public int? TimeToAcknowledge { get; set; }

    public string? StatusEdited { get; set; }

    public long? RowNumber { get; set; }
}
