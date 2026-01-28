using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Ruta: Trackii.Infrastructure/Repositories/WorkOrderRepository.cs
using Microsoft.EntityFrameworkCore;
using Trackii.Application.Interfaces;
using Trackii.Domain.Entities;
using Trackii.Infrastructure.Persistence;

namespace Trackii.Infrastructure.Repositories;

public sealed class WorkOrderRepository : IWorkOrderRepository
{
    private readonly TrackiiDbContext _db;

    public WorkOrderRepository(TrackiiDbContext db)
    {
        _db = db;
    }

    public async Task<WorkOrder?> GetByIdAsync(uint id, CancellationToken ct = default)
    {
        return await _db.WorkOrders
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id, ct);
    }

    public async Task<WorkOrder?> GetByNumberAsync(string woNumber, CancellationToken ct = default)
    {
        return await _db.WorkOrders
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.WoNumber == woNumber, ct);
    }

    public async Task<WorkOrder?> GetByWipIdAsync(uint wipItemId, CancellationToken ct = default)
    {
        return await (
            from wip in _db.WipItems
            join wo in _db.WorkOrders on wip.WorkOrderId equals wo.Id
            where wip.Id == wipItemId
            select wo
        )
        .AsNoTracking()
        .FirstOrDefaultAsync(ct);
    }

    public async Task AddAsync(WorkOrder workOrder, CancellationToken ct = default)
    {
        await _db.WorkOrders.AddAsync(workOrder, ct);
    }

    public Task UpdateAsync(WorkOrder workOrder, CancellationToken ct = default)
    {
        _db.WorkOrders.Update(workOrder);
        return Task.CompletedTask;
    }
}
