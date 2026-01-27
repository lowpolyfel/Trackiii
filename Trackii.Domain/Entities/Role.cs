using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Common;

namespace Trackii.Domain.Entities;

/// <summary>
/// Role: define permisos configurables del sistema.
/// Reglas de dominio:
/// - El nombre es único.
/// - No es jerárquico.
/// - Un usuario solo puede tener un rol.
/// </summary>
public sealed class Role
{
    public uint Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    // Constructor para rehidratación (BD)
    public Role(uint id, string name)
    {
        Id = id;
        Name = Guard.NotNullOrWhiteSpace(name, nameof(name), maxLen: 50);
    }

    // Factory para creación nueva
    public static Role CreateNew(string name)
        => new(0, name);

    public void Rename(string newName)
    {
        Name = Guard.NotNullOrWhiteSpace(newName, nameof(newName), maxLen: 50);
    }

    public override string ToString()
        => $"Role(Id={Id}, Name={Name})";
}
