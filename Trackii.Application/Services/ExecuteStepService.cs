using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Ruta: Trackii.Application/Services/ExecuteStepService.cs
using Trackii.Application.Interfaces;
using Trackii.Domain.Entities;
using Trackii.Domain.Enums;

namespace Trackii.Application.Services;

public sealed class ExecuteStepService
{
    private readonly IWipItemRepository _wipRepo;
    private readonly IRouteStepRepository _routeStepRepo;
    private readonly IWipStepExecutionRepository _executionRepo;
    private readonly IScanEventRepository _scanRepo;
    private readonly IDeviceRepository _deviceRepo;
    private readonly IWorkOrderRepository _workOrderRepo;
    private readonly IUnitOfWork _uow;
    private readonly IClock _clock;

    public ExecuteStepService(
        IWipItemRepository wipRepo,
        IRouteStepRepository routeStepRepo,
        IWipStepExecutionRepository executionRepo,
        IScanEventRepository scanRepo,
        IDeviceRepository deviceRepo,
        IWorkOrderRepository workOrderRepo,
        IUnitOfWork uow,
        IClock clock)
    {
        _wipRepo = wipRepo;
        _routeStepRepo = routeStepRepo;
        _executionRepo = executionRepo;
        _scanRepo = scanRepo;
        _deviceRepo = deviceRepo;
        _workOrderRepo = workOrderRepo;
        _uow = uow;
        _clock = clock;
    }

    public async Task ExecuteAsync(
        uint wipItemId,
        uint routeStepId,
        uint userId,
        uint deviceId,
        uint locationId,
        uint qtyIn,
        uint qtyScrap,
        CancellationToken ct = default)
    {
        var now = _clock.UtcNow;

        // 1) Auditoría: siempre registramos intento (ENTRY)
        await _scanRepo.AddAsync(
            ScanEvent.CreateNew(wipItemId, routeStepId, ScanType.ENTRY, now),
            ct);

        // 2) Load core entities
        var wip = await _wipRepo.GetByIdAsync(wipItemId, ct);
        if (wip is null)
        {
            await _scanRepo.AddAsync(ScanEvent.CreateNew(wipItemId, routeStepId, ScanType.ERROR, now), ct);
            throw new InvalidOperationException("WIP no encontrado.");
        }

        if (wip.Status != WipStatus.ACTIVE)
        {
            await _scanRepo.AddAsync(ScanEvent.CreateNew(wipItemId, routeStepId, ScanType.ERROR, now), ct);
            throw new InvalidOperationException("WIP no está activo.");
        }

        var step = await _routeStepRepo.GetByIdAsync(routeStepId, ct);
        if (step is null)
        {
            await _scanRepo.AddAsync(ScanEvent.CreateNew(wipItemId, routeStepId, ScanType.ERROR, now), ct);
            throw new InvalidOperationException("RouteStep no existe.");
        }

        // 3) Secuencia estricta
        if (wip.CurrentStepId != step.Id)
        {
            await _scanRepo.AddAsync(ScanEvent.CreateNew(wipItemId, routeStepId, ScanType.ERROR, now), ct);
            throw new InvalidOperationException("Paso incorrecto: no corresponde al current step.");
        }

        // 4) Device ↔ Location ↔ Step
        if (step.LocationId != locationId)
        {
            await _scanRepo.AddAsync(ScanEvent.CreateNew(wipItemId, routeStepId, ScanType.ERROR, now), ct);
            throw new InvalidOperationException("Location incorrecta para este step.");
        }

        var device = await _deviceRepo.GetByIdAsync(deviceId, ct);
        if (device is null || device.LocationId != locationId)
        {
            await _scanRepo.AddAsync(ScanEvent.CreateNew(wipItemId, routeStepId, ScanType.ERROR, now), ct);
            throw new InvalidOperationException("Device no pertenece a la location.");
        }

        // 5) Duplicado
        if (await _executionRepo.ExistsForStepAsync(wip.Id, step.Id, ct))
        {
            await _scanRepo.AddAsync(ScanEvent.CreateNew(wipItemId, routeStepId, ScanType.ERROR, now), ct);
            throw new InvalidOperationException("Este step ya fue ejecutado para este WIP.");
        }

        // 6) Cantidades
        if (qtyIn == 0)
        {
            await _scanRepo.AddAsync(ScanEvent.CreateNew(wipItemId, routeStepId, ScanType.ERROR, now), ct);
            throw new InvalidOperationException("QtyIn debe ser mayor a cero.");
        }

        if (qtyScrap > qtyIn)
        {
            await _scanRepo.AddAsync(ScanEvent.CreateNew(wipItemId, routeStepId, ScanType.ERROR, now), ct);
            throw new InvalidOperationException("QtyScrap no puede ser mayor a QtyIn.");
        }

        var lastExec = await _executionRepo.GetLastExecutionAsync(wip.Id, ct);

        uint qtyDisponible = lastExec is null
            ? wip.QtyInput
            : (lastExec.QtyIn - lastExec.QtyScrap);

        if (qtyIn > qtyDisponible)
        {
            await _scanRepo.AddAsync(ScanEvent.CreateNew(wipItemId, routeStepId, ScanType.ERROR, now), ct);
            throw new InvalidOperationException("QtyIn no puede ser mayor al remanente real.");
        }

        // 7) Transacción
        await _uow.BeginAsync(ct);
        try
        {
            var exec = WipStepExecution.CreateNew(
                wip.Id,
                step.Id,
                userId,
                deviceId,
                locationId,
                qtyIn,
                qtyScrap,
                now);

            await _executionRepo.AddAsync(exec, ct);

            var nextStep = await _routeStepRepo.GetNextStepAsync(step.RouteId, step.StepNumber, ct);

            if (nextStep is null)
            {
                wip.Finish();

                var wo = await _workOrderRepo.GetByWipIdAsync(wip.Id, ct);
                if (wo is not null)
                {
                    wo.Finish();
                    await _workOrderRepo.UpdateAsync(wo, ct);
                }
            }
            else
            {
                wip.MoveToStep(nextStep.Id);
            }

            await _wipRepo.UpdateAsync(wip, ct);

            await _uow.CommitAsync(ct);
        }
        catch
        {
            await _uow.RollbackAsync(ct);
            throw;
        }
    }
}
