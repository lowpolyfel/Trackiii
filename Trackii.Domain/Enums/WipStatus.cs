using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Trackii.Domain.Enums;

/// <summary>
/// Estados del WIP (pieza o lote en proceso).
/// </summary>
public enum WipStatus
{
    ACTIVE,
    HOLD,
    FINISHED,
    SCRAPPED
}
