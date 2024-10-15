using System;
using System.Collections.Generic;

namespace FMSD_BE.Models;

public partial class RfidDevice
{
    public long Id { get; set; }

    public Guid? PumpGuid { get; set; }

    public string? RfidProtocol { get; set; }

    public string? RfidAddress { get; set; }

    public string? RfidModel { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long? LocalId { get; set; }

    public Guid StationGuid { get; set; }

    public virtual Pump? Pump { get; set; }
}
