using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class PersonalAccessToken
{
    public long Id { get; set; }

    public string TokenableType { get; set; } = null!;

    public long TokenableId { get; set; }

    public string Name { get; set; } = null!;

    public string Token { get; set; } = null!;

    public string? Abilities { get; set; }

    public DateTime? LastUsedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
