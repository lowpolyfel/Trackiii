using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Trackii.Application.DTOs.Wip;

public sealed class CreateWipItemRequest
{
    public uint WorkOrderId { get; init; }
    public uint QtyInput { get; init; }
}