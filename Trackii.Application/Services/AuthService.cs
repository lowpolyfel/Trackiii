// Ruta: Trackii.Application/Services/AuthService.cs
using Trackii.Application.Dtos.Auth;
using Trackii.Application.Interfaces;

namespace Trackii.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResultDto> LoginAsync(
        string username,
        string password,
        CancellationToken ct = default)
    {
        var user = await _userRepository.GetByUsernameAsync(username, ct);

        if (user is null)
            throw new InvalidOperationException("Usuario o contraseña incorrectos.");

        if (!user.Active)
            throw new InvalidOperationException("Usuario inactivo.");

        // 👇 AJUSTA AQUÍ AL NOMBRE REAL
        if (!_passwordHasher.Verify(user.PasswordHash, password))
            throw new InvalidOperationException("Usuario o contraseña incorrectos.");

        var role = await _roleRepository.GetByIdAsync(user.RoleId, ct)
            ?? throw new InvalidOperationException("Rol inválido.");

        return new AuthResultDto
        {
            UserId = user.Id,
            Username = user.Username,
            RoleName = role.Name
        };
    }
}
