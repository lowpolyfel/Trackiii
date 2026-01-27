using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Common;

namespace Trackii.Domain.Entities;

/// <summary>
/// Product: Part Number manufacturable.
/// Reglas de dominio:
/// - El part number es único globalmente (validación externa).
/// - Pertenece obligatoriamente a una Subfamily y no puede cambiarse.
/// - Usa siempre la ruta activa de su Subfamily (rework fuera del modelo base).
/// - No existen revisiones del producto.
/// - No puede inactivarse si existe producción activa asociada.
/// </summary>
public sealed class Product
{
    public uint Id { get; private set; }
    public uint SubfamilyId { get; private set; }
    public string PartNumber { get; private set; } = string.Empty;
    public bool Active { get; private set; }

    // Constructor para rehidratación (BD)
    public Product(uint id, uint subfamilyId, string partNumber, bool active = true)
    {
        if (subfamilyId == 0)
            throw new DomainException("Product debe pertenecer a una Subfamily válida.");

        Id = id;
        SubfamilyId = subfamilyId;
        PartNumber = Guard.NotNullOrWhiteSpace(partNumber, nameof(partNumber), maxLen: 50);
        Active = active;
    }

    // Factory para creación nueva (Id lo asigna BD)
    public static Product CreateNew(uint subfamilyId, string partNumber)
        => new(0, subfamilyId, partNumber, active: true);

    /// <summary>
    /// Inactiva el producto si está completamente liberado.
    /// </summary>
    /// <param name="hasActiveProduction">
    /// true si existe cualquier WO/WIP activo asociado a este producto.
    /// </param>
    public void Deactivate(bool hasActiveProduction)
    {
        if (!Active) return;

        if (hasActiveProduction)
            throw new DomainException("No se puede inactivar el producto: existen órdenes o WIP activos asociados.");

        Active = false;
    }

    public void Activate()
    {
        if (Active) return;
        Active = true;
    }

    public override string ToString()
        => $"Product(Id={Id}, SubfamilyId={SubfamilyId}, PartNumber={PartNumber}, Active={Active})";
}

