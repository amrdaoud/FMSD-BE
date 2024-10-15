using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class OauthAuthCode
{
    public string Id { get; set; } = null!;

    public long UserId { get; set; }

    public long ClientId { get; set; }

    public string? Scopes { get; set; }

    public bool Revoked { get; set; }

    public DateTime? ExpiresAt { get; set; }
}
