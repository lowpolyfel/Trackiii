using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackii.Application.DTOs.ExecuteStep;

public sealed class ExecuteStepRequest
{
    public uint WipItemId { get; init; }
    public uint RouteStepId { get; init; }
    public uint UserId { get; init; }
    public uint DeviceId { get; init; }
    public uint LocationId { get; init; }
    public uint QtyIn { get; init; }
    public uint QtyScrap { get; init; }
}