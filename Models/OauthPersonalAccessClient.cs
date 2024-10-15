using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class OauthPersonalAccessClient
{
    public long Id { get; set; }

    public long ClientId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
