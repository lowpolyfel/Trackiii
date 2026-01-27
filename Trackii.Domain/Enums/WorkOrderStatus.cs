using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Trackii.Domain.Enums;

/// <summary>
/// Estados de una orden de trabajo.
/// </summary>
public enum WorkOrderStatus
{
    OPEN,
    IN_PROGRESS,
    FINISHED,
    CANCELLED
}
