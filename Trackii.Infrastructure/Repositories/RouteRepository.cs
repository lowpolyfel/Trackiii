using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Trackii.Application.Interfaces;
using Trackii.Domain.Entities;
using Trackii.Infrastructure.Persistence;

namespace Trackii.Infrastructure.Repositories;

public sealed class RouteRepository : IRouteRepository
{
    private readonly TrackiiDbContext _db;

    public RouteRepository(TrackiiDbContext db)
    {
        _db = db;
    }

    public async Task<Route?> GetActiveBySubfamilyIdAsync(
        uint subfamilyId,
        CancellationToken ct = default)
    {
        return await _db.Routes
            .AsNoTracking()
            .Where(r => r.SubfamilyId == subfamilyId && r.Active)
            .OrderByDescending(r => r.Id) // por seguridad, si existiera más de una
            .FirstOrDefaultAsync(ct);
    }
}
