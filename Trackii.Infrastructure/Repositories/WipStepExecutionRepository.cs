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

public sealed class WipStepExecutionRepository : IWipStepExecutionRepository
{
    private readonly TrackiiDbContext _db;

    public WipStepExecutionRepository(TrackiiDbContext db)
    {
        _db = db;
    }

    public async Task<bool> ExistsForStepAsync(uint wipItemId, uint routeStepId, CancellationToken ct = default)
    {
        return await _db.WipStepExecutions
            .AsNoTracking()
            .AnyAsync(e => e.WipItemId == wipItemId && e.RouteStepId == routeStepId, ct);
    }

    public async Task<WipStepExecution?> GetLastExecutionAsync(uint wipItemId, CancellationToken ct = default)
    {
        return await _db.WipStepExecutions
            .AsNoTracking()
            .Where(e => e.WipItemId == wipItemId)
            .OrderByDescending(e => e.CreatedAt)
            .FirstOrDefaultAsync(ct);
    }

    public async Task AddAsync(WipStepExecution execution, CancellationToken ct = default)
    {
        await _db.WipStepExecutions.AddAsync(execution, ct);
    }
}
