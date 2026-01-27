using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackii.Application.DTOs.ExecuteStep;

public sealed class ExecuteStepResponse
{
    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;

    public uint? NewCurrentStepId { get; init; }
    public bool WorkOrderFinished { get; init; }
}