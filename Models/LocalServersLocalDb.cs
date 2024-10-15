using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class LocalServersLocalDb
{
    public long Id { get; set; }

    public string LocalServer { get; set; } = null!;

    public string LocalDb { get; set; } = null!;
}
