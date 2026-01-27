using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Common;

namespace Trackii.Domain.Entities;

/// <summary>
/// Subfamily: nivel operativo clave.
/// Reglas de dominio:
/// - Pertenece obligatoriamente a una Family.
/// - Es el punto de anclaje del proceso (rutas).
/// - Debe existir exactamente una ruta activa (la validación de unicidad/activa se hace fuera).
/// - El nombre es editable; el ID es la identidad.
/// - No puede inactivarse si existe producción activa asociada.
/// </summary>
public sealed class Subfamily
{
    public uint Id { get; private set; }
    public uint FamilyId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool Active { get; private set; }

    // Constructor para rehidratación (BD)
    public Subfamily(uint id, uint familyId, string name, bool active = true)
    {
        if (familyId == 0)
            throw new DomainException("Subfamily debe pertenecer a una Family válida.");

        Id = id;
        FamilyId = familyId;
        Name = Guard.NotNullOrWhiteSpace(name, nameof(name), maxLen: 100);
        Active = active;
    }

    // Factory para creación nueva (Id lo asigna BD)
    public static Subfamily CreateNew(uint familyId, string name)
        => new(0, familyId, name, active: true);

    public void Rename(string newName)
    {
        Name = Guard.NotNullOrWhiteSpace(newName, nameof(newName), maxLen: 100);
    }

    /// <summary>
    /// Inactiva la subfamily si está completamente liberada.
    /// </summary>
    /// <param name="hasActiveProduction">
    /// true si existe cualquier WO/WIP activo asociado a esta subfamily.
    /// </param>
    public void Deactivate(bool hasActiveProduction)
    {
        if (!Active) return;

        if (hasActiveProduction)
            throw new DomainException("No se puede inactivar la subfamily: existen órdenes o WIP activos asociados.");

        Active = false;
    }

    public void Activate()
    {
        if (Active) return;
        Active = true;
    }

    public override string ToString()
        => $"Subfamily(Id={Id}, FamilyId={FamilyId}, Name={Name}, Active={Active})";
}
