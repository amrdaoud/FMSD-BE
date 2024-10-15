using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class OauthAccessToken
{
    public string Id { get; set; } = null!;

    public long? UserId { get; set; }

    public long ClientId { get; set; }

    public string? Name { get; set; }

    public string? Scopes { get; set; }

    public bool Revoked { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }
}
