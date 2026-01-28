using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackii.Application.DTOs.WorkOrder;

public sealed class CreateWorkOrderRequest
{
    public string WoNumber { get; init; } = string.Empty;
    public uint ProductId { get; init; }
}
