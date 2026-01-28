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

public sealed class WipItemRepository : IWipItemRepository
{
    private readonly TrackiiDbContext _db;

    public WipItemRepository(TrackiiDbContext db)
    {
        _db = db;
    }

    public async Task<WipItem?> GetByIdAsync(uint id, CancellationToken ct = default)
    {
        return await _db.WipItems
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id, ct);
    }

    public async Task AddAsync(WipItem wipItem, CancellationToken ct = default)
    {
        await _db.WipItems.AddAsync(wipItem, ct);
    }

    public Task UpdateAsync(WipItem wipItem, CancellationToken ct = default)
    {
        _db.WipItems.Update(wipItem);
        return Task.CompletedTask;
    }
}
