using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Application.Interfaces;
using Trackii.Domain.Entities;
using Trackii.Infrastructure.Persistence;

namespace Trackii.Infrastructure.Repositories;

public sealed class ScanEventRepository : IScanEventRepository
{
    private readonly TrackiiDbContext _db;

    public ScanEventRepository(TrackiiDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(
        ScanEvent scanEvent,
        CancellationToken ct = default)
    {
        await _db.ScanEvents.AddAsync(scanEvent, ct);
    }
}
