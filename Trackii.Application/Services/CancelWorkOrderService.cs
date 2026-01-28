using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Application.DTOs.WorkOrder;
using Trackii.Application.Interfaces;
using Trackii.Domain.Enums;

namespace Trackii.Application.Services;

public sealed class CancelWorkOrderService
{
    private readonly IWorkOrderRepository _woRepo;
    private readonly IWipItemRepository _wipRepo;
    private readonly IUnitOfWork _uow;

    public CancelWorkOrderService(
        IWorkOrderRepository woRepo,
        IWipItemRepository wipRepo,
        IUnitOfWork uow)
    {
        _woRepo = woRepo;
        _wipRepo = wipRepo;
        _uow = uow;
    }

    public async Task ExecuteAsync(
        CancelWorkOrderRequest request,
        CancellationToken ct = default)
    {
        var wo = await _woRepo.GetByIdAsync(request.WorkOrderId, ct)
            ?? throw new InvalidOperationException("WorkOrder no existe.");

        if (wo.Status == WorkOrderStatus.CANCELLED)
            return;

        await _uow.BeginAsync(ct);
        try
        {
            wo.Cancel();
            await _woRepo.UpdateAsync(wo, ct);

            var wip = await _wipRepo.GetByIdAsync(wo.Id, ct);
            if (wip is not null && wip.Status != WipStatus.FINISHED)
            {
                wip.Scrap();
                await _wipRepo.UpdateAsync(wip, ct);
            }

            await _uow.CommitAsync(ct);
        }
        catch
        {
            await _uow.RollbackAsync(ct);
            throw;
        }
    }
}