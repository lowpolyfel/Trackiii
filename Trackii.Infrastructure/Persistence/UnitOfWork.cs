using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Application.Interfaces;

namespace Trackii.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TrackiiDbContext _db;

    public UnitOfWork(TrackiiDbContext db)
    {
        _db = db;
    }

    public async Task BeginAsync(CancellationToken ct = default)
    {
        if (_db.Database.CurrentTransaction is null)
            await _db.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitAsync(CancellationToken ct = default)
    {
        await _db.SaveChangesAsync(ct);

        if (_db.Database.CurrentTransaction is not null)
            await _db.Database.CurrentTransaction.CommitAsync(ct);
    }

    public async Task RollbackAsync(CancellationToken ct = default)
    {
        if (_db.Database.CurrentTransaction is not null)
            await _db.Database.CurrentTransaction.RollbackAsync(ct);
    }
}
