using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Common;

namespace Trackii.Domain.Entities;

/// <summary>
/// Area: clasificación (no operativa).
/// Reglas clave:
/// - Nombre no duplicado (se valida en capa de aplicación/infra o reglas globales).
/// - ID es la identidad; el nombre es editable.
/// - No puede inactivarse si existe producción activa (WIP/WO activos) asociada.
/// </summary>
public sealed class Area
{
    public uint Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool Active { get; private set; }

    // Constructor para rehidratación (BD)
    public Area(uint id, string name, bool active = true)
    {
        Id = id;
        Name = Guard.NotNullOrWhiteSpace(name, nameof(name), maxLen: 100);
        Active = active;
    }

    // Factory para creación nueva (Id lo asigna BD)
    public static Area CreateNew(string name)
        => new(0, name, active: true);

    public void Rename(string newName)
    {
        Name = Guard.NotNullOrWhiteSpace(newName, nameof(newName), maxLen: 100);
    }

    /// <summary>
    /// Inactiva el área si está completamente liberada.
    /// </summary>
    /// <param name="hasActiveProduction">
    /// true si existe cualquier WO/WIP activo asociado a esta área (directa o indirectamente).
    /// </param>
    public void Deactivate(bool hasActiveProduction)
    {
        if (!Active) return;

        if (hasActiveProduction)
            throw new DomainException("No se puede inactivar el área: existen órdenes o WIP activos asociados.");

        Active = false;
    }

    /// <summary>
    /// Reactiva el área (si el negocio lo permite).
    /// </summary>
    public void Activate()
    {
        if (Active) return;
        Active = true;
    }

    public override string ToString() => $"Area(Id={Id}, Name={Name}, Active={Active})";
}
