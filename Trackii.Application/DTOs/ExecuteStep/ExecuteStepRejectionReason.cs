using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackii.Application.DTOs.ExecuteStep;

public enum ExecuteStepRejectionReason
{
    None = 0,
    WipNotFound,
    WipNotActive,
    StepNotCurrent,
    StepAlreadyExecuted,
    DeviceLocationMismatch,
    InvalidQuantities,
    RouteStepNotFound,
    InternalError
}