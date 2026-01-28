using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Application.DTOs.Wip;
using Trackii.Application.Interfaces;
using Trackii.Domain.Entities;
using Trackii.Domain.Enums;

namespace Trackii.Application.Services;

public sealed class CreateWipItemService
{
    private readonly IWorkOrderRepository _woRepo;
    private readonly IRouteRepository _routeRepo;
    private readonly IRouteStepRepository _routeStepRepo;
    private readonly IWipItemRepository _wipRepo;
    private readonly IUnitOfWork _uow;
    private readonly IClock _clock;

    public CreateWipItemService(
        IWorkOrderRepository woRepo,
        IRouteRepository routeRepo,
        IRouteStepRepository routeStepRepo,
        IWipItemRepository wipRepo,
        IUnitOfWork uow,
        IClock clock)
    {
        _woRepo = woRepo;
        _routeRepo = routeRepo;
        _routeStepRepo = routeStepRepo;
        _wipRepo = wipRepo;
        _uow = uow;
        _clock = clock;
    }

    public async Task<CreateWipItemResponse> ExecuteAsync(
        CreateWipItemRequest request,
        CancellationToken ct = default)
    {
        if (request.QtyInput == 0)
            throw new InvalidOperationException("QtyInput debe ser mayor a cero.");

        var wo = await _woRepo.GetByIdAsync(request.WorkOrderId, ct)
            ?? throw new InvalidOperationException("WorkOrder no existe.");

        if (wo.Status != WorkOrderStatus.OPEN && wo.Status != WorkOrderStatus.IN_PROGRESS)
            throw new InvalidOperationException("WorkOrder no permite crear WIP.");

        var route = await _routeRepo.GetActiveBySubfamilyIdAsync(
            (await _productSubfamilyAsync(wo, ct)),
            ct) ?? throw new InvalidOperationException("No hay ruta activa.");

        var firstStep = await _routeStepRepo.GetFirstStepAsync(route.Id, ct)
            ?? throw new InvalidOperationException("La ruta no tiene pasos.");

        var wip = WipItem.CreateNew(
            wo.Id,
            firstStep.Id,
            request.QtyInput,
            _clock.UtcNow);

        await _uow.BeginAsync(ct);
        try
        {
            await _wipRepo.AddAsync(wip, ct);
            if (wo.Status == WorkOrderStatus.OPEN)
            {
                wo.Start();
                await _woRepo.UpdateAsync(wo, ct);
            }
            await _uow.CommitAsync(ct);
        }
        catch
        {
            await _uow.RollbackAsync(ct);
            throw;
        }

        return new CreateWipItemResponse
        {
            WipItemId = wip.Id,
            FirstStepId = firstStep.Id
        };
    }

    // helper local: resolver subfamily del producto (contrato asumido)
    private async Task<uint> _productSubfamilyAsync(WorkOrder wo, CancellationToken ct)
    {
        // Este método asume que la infraestructura resuelve la subfamily
        // del producto de la WO. Se mantiene aquí para no romper contratos.
        throw new NotImplementedException("Resolver subfamily del producto en infraestructura.");
    }
}