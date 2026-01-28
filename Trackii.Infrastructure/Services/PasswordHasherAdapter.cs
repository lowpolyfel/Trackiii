using Microsoft.AspNetCore.Identity;
using Trackii.Application.Interfaces;
using Trackii.Domain.Entities;

namespace Trackii.Infrastructure.Services;

public sealed class PasswordHasherAdapter : IPasswordHasher
{
    private readonly PasswordHasher<User> _hasher = new();

    public bool Verify(string hashedPassword, string providedPassword)
    {
        // roleId NO puede ser 0 por reglas de dominio.
        // Usamos 1 (o cualquier >0). No se persiste.
        var dummyUser = new User(
            0,
            string.Empty,
            hashedPassword,
            1,
            true
        );

        var result = _hasher.VerifyHashedPassword(
            dummyUser,
            hashedPassword,
            providedPassword
        );

        return result == PasswordVerificationResult.Success;
    }
}
