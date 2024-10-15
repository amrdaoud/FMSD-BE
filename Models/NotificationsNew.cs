using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class NotificationsNew
{
    public long Id { get; set; }

    public string Type { get; set; } = null!;

    public string NotifiableType { get; set; } = null!;

    public long NotifiableId { get; set; }

    public string Data { get; set; } = null!;

    public DateTime? ReadAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid StationGuid { get; set; }

    public Guid LocalId { get; set; }

    public virtual Station Station { get; set; } = null!;
}
