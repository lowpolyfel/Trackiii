using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Common;

namespace Trackii.Domain.Entities;

/// <summary>
/// RouteStep: paso secuencial obligatorio dentro de una Route.
/// Reglas de dominio:
/// - Pertenece obligatoriamente a una Route.
/// - Define un orden absoluto mediante StepNumber.
/// - Todos los pasos son obligatorios.
/// - No existen pasos paralelos.
/// - No se permite saltar pasos (validación fuera de esta entidad).
/// - El retrabajo no se modela aquí.
/// </summary>
public sealed class RouteStep
{
    public uint Id { get; private set; }
    public uint RouteId { get; private set; }
    public uint LocationId { get; private set; }
    public uint StepNumber { get; private set; }

    // Constructor para rehidratación (BD)
    public RouteStep(
        uint id,
        uint routeId,
        uint stepNumber,
        uint locationId)
    {
        if (routeId == 0)
            throw new DomainException("RouteStep debe pertenecer a una Route válida.");

        if (locationId == 0)
            throw new DomainException("RouteStep debe tener una Location válida.");

        if (stepNumber == 0)
            throw new DomainException("StepNumber debe ser mayor a cero.");

        Id = id;
        RouteId = routeId;
        StepNumber = stepNumber;
        LocationId = locationId;
    }

    // Factory para creación nueva (Id lo asigna BD)
    public static RouteStep CreateNew(
        uint routeId,
        uint stepNumber,
        uint locationId)
        => new(0, routeId, stepNumber, locationId);

    /// <summary>
    /// Permite cambiar la Location asociada al paso.
    /// El orden (StepNumber) no debería cambiar una vez que existen WIP,
    /// pero esa validación se realiza fuera de esta entidad.
    /// </summary>
    public void ChangeLocation(uint newLocationId)
    {
        if (newLocationId == 0)
            throw new DomainException("Location inválida.");

        LocationId = newLocationId;
    }

    /// <summary>
    /// Permite cambiar el número de paso.
    /// Usar solo cuando NO exista producción asociada.
    /// </summary>
    public void ChangeStepNumber(uint newStepNumber)
    {
        if (newStepNumber == 0)
            throw new DomainException("StepNumber debe ser mayor a cero.");

        StepNumber = newStepNumber;
    }

    public override string ToString()
        => $"RouteStep(Id={Id}, RouteId={RouteId}, StepNumber={StepNumber}, LocationId={LocationId})";
}
