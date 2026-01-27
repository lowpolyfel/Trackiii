using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Trackii.Domain.Enums;

/// <summary>
/// Tipo de escaneo registrado por un device.
/// </summary>
public enum ScanType
{
    ENTRY,
    EXIT,
    ERROR,
    MANUAL
}
