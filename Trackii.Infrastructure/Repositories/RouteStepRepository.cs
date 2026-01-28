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

public sealed class RouteStepRepository : IRouteStepRepository
{
    private readonly TrackiiDbContext _db;

    public RouteStepRepository(TrackiiDbContext db)
    {
        _db = db;
    }

    public async Task<RouteStep?> GetByIdAsync(uint id, CancellationToken ct = default)
    {
        return await _db.RouteSteps
            .AsNoTracking()
            .FirstOrDefaultAsync(rs => rs.Id == id, ct);
    }

    public async Task<RouteStep?> GetFirstStepAsync(uint routeId, CancellationToken ct = default)
    {
        return await _db.RouteSteps
            .AsNoTracking()
            .Where(rs => rs.RouteId == routeId)
            .OrderBy(rs => rs.StepNumber)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<RouteStep?> GetNextStepAsync(
        uint routeId,
        uint currentStepNumber,
        CancellationToken ct = default)
    {
        return await _db.RouteSteps
            .AsNoTracking()
            .Where(rs =>
                rs.RouteId == routeId &&
                rs.StepNumber > currentStepNumber)
            .OrderBy(rs => rs.StepNumber)
            .FirstOrDefaultAsync(ct);
    }
}
