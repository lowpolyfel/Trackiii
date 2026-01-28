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

    /// <summary>
    /// Cantidad de piezas que LLEGAN a este step desde el anterior.
    /// En el primer step debe ser <= WipItem.QtyInput.
    /// </summary>
    public uint QtyIn { get; init; }

    /// <summary>
    /// Scrap generado únicamente en este step.
    /// </summary>
    public uint QtyScrap { get; init; }
}