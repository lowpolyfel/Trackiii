using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Common;

namespace Trackii.Domain.Entities;

/// <summary>
/// Route: define el flujo secuencial completo de manufactura para una Subfamily.
/// Reglas de dominio:
/// - Pertenece obligatoriamente a una Subfamily.
/// - Solo puede existir UNA ruta activa por Subfamily (regla validada fuera del dominio).
/// - No puede desactivarse si existen WIP activos usando esta ruta.
/// - El versionado permite cambiar el flujo y volver a versiones anteriores.
/// - Cambiar el estado de la ruta NO afecta WIP ya creados.
/// </summary>
public sealed class Route
{
    public uint Id { get; private set; }
    public uint SubfamilyId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Version { get; private set; } = string.Empty;
    public bool Active { get; private set; }

    // Constructor para rehidratación (BD)
    public Route(
        uint id,
        uint subfamilyId,
        string name,
        string version,
        bool active = true)
    {
        if (subfamilyId == 0)
            throw new DomainException("Route debe pertenecer a una Subfamily válida.");

        Id = id;
        SubfamilyId = subfamilyId;
        Name = Guard.NotNullOrWhiteSpace(name, nameof(name), maxLen: 100);
        Version = Guard.NotNullOrWhiteSpace(version, nameof(version), maxLen: 20);
        Active = active;
    }

    // Factory para creación nueva (Id lo asigna BD)
    public static Route CreateNew(
        uint subfamilyId,
        string name,
        string version)
        => new(0, subfamilyId, name, version, active: true);

    public void Rename(string newName)
    {
        Name = Guard.NotNullOrWhiteSpace(newName, nameof(newName), maxLen: 100);
    }

    public void ChangeVersion(string newVersion)
    {
        Version = Guard.NotNullOrWhiteSpace(newVersion, nameof(newVersion), maxLen: 20);
    }

    /// <summary>
    /// Desactiva la ruta si no existen WIP activos asociados.
    /// </summary>
    /// <param name="hasActiveWip">
    /// true si existen WIP activos utilizando esta ruta.
    /// </param>
    public void Deactivate(bool hasActiveWip)
    {
        if (!Active) return;

        if (hasActiveWip)
            throw new DomainException(
                "No se puede desactivar la ruta: existen WIP activos asociados.");

        Active = false;
    }

    /// <summary>
    /// Activa la ruta.
    /// La validación de que no exista otra ruta activa para la Subfamily
    /// se realiza fuera del dominio.
    /// </summary>
    public void Activate()
    {
        if (Active) return;
        Active = true;
    }

    public override string ToString()
        => $"Route(Id={Id}, SubfamilyId={SubfamilyId}, Name={Name}, Version={Version}, Active={Active})";
}
