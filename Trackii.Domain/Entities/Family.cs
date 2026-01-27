using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Common;

namespace Trackii.Domain.Entities;

/// <summary>
/// Family: organización dentro de un Area (no define proceso).
/// Reglas de dominio:
/// - Pertenece obligatoriamente a un Area.
/// - El nombre es único dentro del Area (la unicidad se valida fuera del dominio).
/// - El nombre es editable; el ID es la identidad.
/// - No puede inactivarse si existe producción activa asociada.
/// </summary>
public sealed class Family
{
    public uint Id { get; private set; }
    public uint AreaId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool Active { get; private set; }

    // Constructor para rehidratación (BD)
    public Family(uint id, uint areaId, string name, bool active = true)
    {
        if (areaId == 0)
            throw new DomainException("Family debe pertenecer a un Area válida.");

        Id = id;
        AreaId = areaId;
        Name = Guard.NotNullOrWhiteSpace(name, nameof(name), maxLen: 100);
        Active = active;
    }

    // Factory para creación nueva (Id lo asigna BD)
    public static Family CreateNew(uint areaId, string name)
        => new(0, areaId, name, active: true);

    public void Rename(string newName)
    {
        Name = Guard.NotNullOrWhiteSpace(newName, nameof(newName), maxLen: 100);
    }

    /// <summary>
    /// Inactiva la familia si está completamente liberada.
    /// </summary>
    /// <param name="hasActiveProduction">
    /// true si existe cualquier WO/WIP activo asociado a esta familia (directa o indirectamente).
    /// </param>
    public void Deactivate(bool hasActiveProduction)
    {
        if (!Active) return;

        if (hasActiveProduction)
            throw new DomainException("No se puede inactivar la familia: existen órdenes o WIP activos asociados.");

        Active = false;
    }

    public void Activate()
    {
        if (Active) return;
        Active = true;
    }

    public override string ToString()
        => $"Family(Id={Id}, AreaId={AreaId}, Name={Name}, Active={Active})";
}
