using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Trackii.Application.DTOs.Wip;

public sealed class CreateWipItemResponse
{
    public uint WipItemId { get; init; }
    public uint FirstStepId { get; init; }
}