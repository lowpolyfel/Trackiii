using Microsoft.EntityFrameworkCore;
using Trackii.Application.Interfaces;
using Trackii.Domain.Entities;
using Trackii.Infrastructure.Persistence;

namespace Trackii.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly TrackiiDbContext _db;

    public UserRepository(TrackiiDbContext db)
    {
        _db = db;
    }

    public async Task<User?> GetByIdAsync(uint id, CancellationToken ct = default)
    {
        // Importante: NO filtramos por Active aquí.
        // AuthService decide qué hacer si está inactivo.
        return await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        return await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username, ct);
    }

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        await _db.Users.AddAsync(user, ct);
    }

    public Task UpdateAsync(User user, CancellationToken ct = default)
    {
        _db.Users.Update(user);
        return Task.CompletedTask;
    }
}
