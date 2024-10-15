using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class UpdateLogsView
{
    public string UserName { get; set; } = null!;

    public string Setting { get; set; } = null!;

    public string Before { get; set; } = null!;

    public string After { get; set; } = null!;

    public Guid StationGuid { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string City { get; set; } = null!;

    public string StationName { get; set; } = null!;

    public long? RowNumber { get; set; }
}
