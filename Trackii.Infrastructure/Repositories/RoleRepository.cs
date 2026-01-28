using Microsoft.EntityFrameworkCore;
using Trackii.Application.Interfaces;
using Trackii.Domain.Entities;
using Trackii.Infrastructure.Persistence;

namespace Trackii.Infrastructure.Repositories;

public sealed class RoleRepository : IRoleRepository
{
    private readonly TrackiiDbContext _db;

    public RoleRepository(TrackiiDbContext db)
    {
        _db = db;
    }

    public async Task<Role?> GetByIdAsync(uint id, CancellationToken ct = default)
    {
        return await _db.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    public async Task AddAsync(Role role, CancellationToken ct = default)
    {
        await _db.Roles.AddAsync(role, ct);
    }
}
