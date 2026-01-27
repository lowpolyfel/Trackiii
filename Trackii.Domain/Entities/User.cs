using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Common;

namespace Trackii.Domain.Entities;

/// <summary>
/// User: usuario del sistema.
/// Reglas de dominio:
/// - Tiene exactamente un Role.
/// - El username es único.
/// - Puede ejecutar pasos según su rol.
/// - No se manejan turnos.
/// </summary>
public sealed class User
{
    public uint Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public uint RoleId { get; private set; }
    public bool Active { get; private set; }

    // Constructor para rehidratación (BD)
    public User(
        uint id,
        string username,
        string passwordHash,
        uint roleId,
        bool active = true)
    {
        if (roleId == 0)
            throw new DomainException("User debe tener un Role válido.");

        Id = id;
        Username = Guard.NotNullOrWhiteSpace(username, nameof(username), maxLen: 50);
        PasswordHash = Guard.NotNullOrWhiteSpace(passwordHash, nameof(passwordHash), maxLen: 255);
        RoleId = roleId;
        Active = active;
    }

    // Factory para creación nueva
    public static User CreateNew(
        string username,
        string passwordHash,
        uint roleId)
        => new(0, username, passwordHash, roleId, active: true);

    public void ChangeRole(uint newRoleId)
    {
        if (newRoleId == 0)
            throw new DomainException("Role inválido.");

        RoleId = newRoleId;
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = Guard.NotNullOrWhiteSpace(
            newPasswordHash,
            nameof(newPasswordHash),
            maxLen: 255);
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
        => $"User(Id={Id}, Username={Username}, RoleId={RoleId}, Active={Active})";
}
