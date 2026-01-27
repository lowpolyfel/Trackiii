using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Common;
using Trackii.Domain.Enums;

namespace Trackii.Domain.Entities;

/// <summary>
/// ScanEvent: log crudo de intentos de escaneo.
/// Reglas de dominio:
/// - Es solo auditoría, no estado.
/// - Puede representar escaneos inválidos.
/// - No todos los ScanEvent generan una ejecución.
/// - Es inmutable una vez creado.
/// </summary>
public sealed class ScanEvent
{
    public uint Id { get; private set; }
    public uint WipItemId { get; private set; }
    public uint RouteStepId { get; private set; }
    public ScanType ScanType { get; private set; }
    public DateTime Timestamp { get; private set; }

    // Constructor para rehidratación (BD)
    public ScanEvent(
        uint id,
        uint wipItemId,
        uint routeStepId,
        ScanType scanType,
        DateTime timestamp)
    {
        if (wipItemId == 0)
            throw new DomainException("ScanEvent debe pertenecer a un WipItem válido.");

        if (routeStepId == 0)
            throw new DomainException("ScanEvent debe tener un RouteStep válido.");

        Id = id;
        WipItemId = wipItemId;
        RouteStepId = routeStepId;
        ScanType = scanType;
        Timestamp = timestamp;
    }

    // Factory para creación nueva (Id lo asigna BD)
    public static ScanEvent CreateNew(
        uint wipItemId,
        uint routeStepId,
        ScanType scanType,
        DateTime timestamp)
        => new(
            0,
            wipItemId,
            routeStepId,
            scanType,
            timestamp);

    public override string ToString()
        => $"ScanEvent(Id={Id}, WipItemId={WipItemId}, Step={RouteStepId}, Type={ScanType}, Ts={Timestamp:O})";
}
