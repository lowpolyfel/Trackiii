using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Common;

namespace Trackii.Domain.Entities;

/// <summary>
/// Token: credencial para autenticación en tabletas.
/// Reglas de dominio:
/// - Se usa exclusivamente para tablets.
/// - Es único.
/// - Controla qué usuarios pueden operar devices.
/// - No reemplaza login/password web.
/// </summary>
public sealed class Token
{
    public uint Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;

    // Constructor para rehidratación (BD)
    public Token(uint id, string name, string code)
    {
        Id = id;
        Name = Guard.NotNullOrWhiteSpace(name, nameof(name), maxLen: 50);
        Code = Guard.NotNullOrWhiteSpace(code, nameof(code), maxLen: 50);
    }

    // Factory para creación nueva
    public static Token CreateNew(string name, string code)
        => new(0, name, code);

    public void Rename(string newName)
    {
        Name = Guard.NotNullOrWhiteSpace(newName, nameof(newName), maxLen: 50);
    }

    public void RotateCode(string newCode)
    {
        Code = Guard.NotNullOrWhiteSpace(newCode, nameof(newCode), maxLen: 50);
    }

    public override string ToString()
        => $"Token(Id={Id}, Name={Name})";
}
