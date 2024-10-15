using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class NotificationVw
{
    public string? NotificationType { get; set; }

    public string Type { get; set; } = null!;

    public string? Show { get; set; }

    public string? Description { get; set; }

    public long? NotificationTypeId { get; set; }

    public string? UserName { get; set; }

    public string StationName { get; set; } = null!;

    public string City { get; set; } = null!;

    public long? AcknowledgedUserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? AcknowledgedUsername { get; set; }

    public long? RowNumber { get; set; }
}
