using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Common;

namespace Trackii.Domain.Entities;

/// <summary>
/// Device: tablet Android usada para escaneo.
/// Reglas de dominio:
/// - device_uid es único y representa el Android ID.
/// - Está asociado a una Location (puede cambiar).
/// - Un device inactivo no puede registrar escaneos.
/// - La validación de que el device coincida con el step actual del WIP
///   se realiza fuera de esta entidad.
/// </summary>
public sealed class Device
{
    public uint Id { get; private set; }
    public string DeviceUid { get; private set; } = string.Empty;
    public uint LocationId { get; private set; }
    public string? Name { get; private set; }
    public bool Active { get; private set; }

    // Constructor para rehidratación (BD)
    public Device(
        uint id,
        string deviceUid,
        uint locationId,
        string? name,
        bool active = true)
    {
        if (locationId == 0)
            throw new DomainException("Device debe estar asociado a una Location válida.");

        Id = id;
        DeviceUid = Guard.NotNullOrWhiteSpace(deviceUid, nameof(deviceUid), maxLen: 100);
        LocationId = locationId;
        Name = string.IsNullOrWhiteSpace(name) ? null : name.Trim();
        Active = active;
    }

    // Factory para creación nueva (Id lo asigna BD)
    public static Device CreateNew(
        string deviceUid,
        uint locationId,
        string? name = null)
        => new(0, deviceUid, locationId, name, active: true);

    /// <summary>
    /// Cambia la location del device (reubicación física).
    /// </summary>
    public void ChangeLocation(uint newLocationId)
    {
        if (newLocationId == 0)
            throw new DomainException("Location inválida.");

        LocationId = newLocationId;
    }

    public void Rename(string? newName)
    {
        Name = string.IsNullOrWhiteSpace(newName) ? null : newName.Trim();
    }

    public void Deactivate()
    {
        if (!Active) return;
        Active = false;
    }

    public void Activate()
    {
        if (Active) return;
        Active = true;
    }

    public override string ToString()
        => $"Device(Id={Id}, DeviceUid={DeviceUid}, LocationId={LocationId}, Active={Active})";
}
