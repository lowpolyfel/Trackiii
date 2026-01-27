using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Common;

namespace Trackii.Domain.Entities;

/// <summary>
/// Location: lugar físico donde ocurre un proceso.
/// Reglas de dominio:
/// - Representa una ubicación física operativa.
/// - Puede abstraer proceso y lugar físico como un solo concepto.
/// - Usualmente tiene 1 device asignado (regla operativa, no forzada aquí).
/// - No puede inactivarse si existe producción activa asociada.
/// - Una location inactiva no permite escaneos.
/// </summary>
public sealed class Location
{
    public uint Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool Active { get; private set; }

    // Constructor para rehidratación (BD)
    public Location(uint id, string name, bool active = true)
    {
        Id = id;
        Name = Guard.NotNullOrWhiteSpace(name, nameof(name), maxLen: 100);
        Active = active;
    }

    // Factory para creación nueva (Id lo asigna BD)
    public static Location CreateNew(string name)
        => new(0, name, active: true);

    public void Rename(string newName)
    {
        Name = Guard.NotNullOrWhiteSpace(newName, nameof(newName), maxLen: 100);
    }

    /// <summary>
    /// Inactiva la location si está completamente liberada.
    /// </summary>
    /// <param name="hasActiveProduction">
    /// true si existen WIP/WO activos usando esta location.
    /// </param>
    public void Deactivate(bool hasActiveProduction)
    {
        if (!Active) return;

        if (hasActiveProduction)
            throw new DomainException(
                "No se puede inactivar la location: existen WIP u órdenes activas asociadas.");

        Active = false;
    }

    public void Activate()
    {
        if (Active) return;
        Active = true;
    }

    public override string ToString()
        => $"Location(Id={Id}, Name={Name}, Active={Active})";
}
