using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Common;

namespace Trackii.Domain.Entities;

/// <summary>
/// WipStepExecution: historial (bitácora) de ejecución de un paso.
/// Reglas de dominio:
/// - Representa un evento REAL que ocurrió.
/// - Pertenece a un WipItem.
/// - Hay EXACTAMENTE una ejecución por Step por WIP (1:1).
/// - No existen ejecuciones parciales.
/// - Al crearse, provoca el avance del WIP (fuera de esta entidad).
/// - Registra quién, dónde y con qué device se trabajó.
/// </summary>
public sealed class WipStepExecution
{
    public uint Id { get; private set; }
    public uint WipItemId { get; private set; }
    public uint RouteStepId { get; private set; }
    public uint UserId { get; private set; }
    public uint DeviceId { get; private set; }
    public uint LocationId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public uint QtyIn { get; private set; }
    public uint QtyScrap { get; private set; }

    // Constructor para rehidratación (BD)
    public WipStepExecution(
        uint id,
        uint wipItemId,
        uint routeStepId,
        uint userId,
        uint deviceId,
        uint locationId,
        DateTime createdAt,
        uint qtyIn,
        uint qtyScrap)
    {
        if (wipItemId == 0)
            throw new DomainException("WipStepExecution debe pertenecer a un WipItem válido.");

        if (routeStepId == 0)
            throw new DomainException("WipStepExecution debe tener un RouteStep válido.");

        if (userId == 0)
            throw new DomainException("WipStepExecution debe tener un User válido.");

        if (deviceId == 0)
            throw new DomainException("WipStepExecution debe tener un Device válido.");

        if (locationId == 0)
            throw new DomainException("WipStepExecution debe tener una Location válida.");

        if (qtyIn == 0)
            throw new DomainException("QtyIn debe ser mayor a cero.");

        if (qtyScrap > qtyIn)
            throw new DomainException("QtyScrap no puede ser mayor que QtyIn.");

        Id = id;
        WipItemId = wipItemId;
        RouteStepId = routeStepId;
        UserId = userId;
        DeviceId = deviceId;
        LocationId = locationId;
        CreatedAt = createdAt;
        QtyIn = qtyIn;
        QtyScrap = qtyScrap;
    }

    // Factory para creación nueva (Id lo asigna BD)
    public static WipStepExecution CreateNew(
        uint wipItemId,
        uint routeStepId,
        uint userId,
        uint deviceId,
        uint locationId,
        uint qtyIn,
        uint qtyScrap,
        DateTime createdAt)
        => new(
            0,
            wipItemId,
            routeStepId,
            userId,
            deviceId,
            locationId,
            createdAt,
            qtyIn,
            qtyScrap);

    public uint QtyGood => QtyIn - QtyScrap;

    public override string ToString()
        => $"WipStepExecution(Id={Id}, WipItemId={WipItemId}, Step={RouteStepId}, QtyIn={QtyIn}, Scrap={QtyScrap})";
}
