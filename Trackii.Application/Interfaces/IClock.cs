using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackii.Application.Interfaces;

/// <summary>
/// Proveedor de tiempo para hacer el sistema testeable.
/// </summary>
public interface IClock
{
    DateTime UtcNow { get; }
}
