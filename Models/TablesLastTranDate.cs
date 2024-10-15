using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class TablesLastTranDate
{
    public string ServerName { get; set; } = null!;

    public string TableName { get; set; } = null!;

    public DateTime? LastTranDate { get; set; }
}
