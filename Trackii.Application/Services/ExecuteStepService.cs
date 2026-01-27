using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Application.DTOs.ExecuteStep;
using Trackii.Application.Interfaces;
using Trackii.Domain.Entities;
using Trackii.Domain.Enums;

namespace Trackii.Application.Services;

/// <summary>
/// Caso de uso core:
/// Ejecuta un intento de escaneo y, si es válido,
/// registra la ejecución y avanza el WIP.
/// </summary>
public sealed class ExecuteStepService
{
    private readonly IWipItemRepository _wipRepo;
    private readonly IRouteStepRepository _routeStepRepo;
    private readonly IWipStepExecutionRepository _executionRepo;
    private readonly IScanEventRepository _scanRepo;
    private readonly IWorkOrderRepository _workOrderRepo;
    private readonly IUnitOfWork _uow;
    private readonly IClock _clock;

    public ExecuteStepService(
        IWipItemRepository wipRepo,
        IRouteStepRepository routeStepRepo,
        IWipStepExecutionRepository executionRepo,
        IScanEventRepository scanRepo,
        IWorkOrderRepository workOrderRepo,
        IUnitOfWork uow,
        IClock clock)
    {
        _wipRepo = wipRepo;
        _routeStepRepo = routeStepRepo;
        _executionRepo = executionRepo;
        _scanRepo = scanRepo;
        _workOrderRepo = workOrderRepo;
        _uow = uow;
        _clock = clock;
    }

    public async Task<ExecuteStepResponse> ExecuteAsync(
        ExecuteStepRequest request,
        CancellationToken ct = default)
    {
        // 1️⃣ Siempre registramos ScanEvent (auditoría)
        var scanEvent = ScanEvent.CreateNew(
            request.WipItemId,
            request.RouteStepId,
            ScanType.ENTRY,
            _clock.UtcNow);

        await _scanRepo.AddAsync(scanEvent, ct);

        // 2️⃣ Cargar WIP
        var wip = await _wipRepo.GetByIdAsync(request.WipItemId, ct);
        if (wip is null)
            return Reject("WIP no encontrado", ExecuteStepRejectionReason.WipNotFound);

        if (wip.Status != WipStatus.ACTIVE)
            return Reject("WIP no está activo", ExecuteStepRejectionReason.WipNotActive);

        // 3️⃣ Cargar RouteStep
        var routeStep = await _routeStepRepo.GetByIdAsync(request.RouteStepId, ct);
        if (routeStep is null)
            return Reject("RouteStep no existe", ExecuteStepRejectionReason.RouteStepNotFound);

        // 4️⃣ Validar step actual
        if (wip.CurrentStepId != routeStep.Id)
            return Reject("El paso no corresponde al step actual del WIP",
                ExecuteStepRejectionReason.StepNotCurrent);

        // 5️⃣ Validar que no se haya ejecutado antes
        var alreadyExecuted = await _executionRepo.ExistsForStepAsync(
            wip.Id,
            routeStep.Id,
            ct);

        if (alreadyExecuted)
            return Reject("El step ya fue ejecutado para este WIP",
                ExecuteStepRejectionReason.StepAlreadyExecuted);

        // 6️⃣ Validar cantidades
        if (request.QtyIn == 0 || request.QtyScrap > request.QtyIn)
            return Reject("Cantidades inválidas",
                ExecuteStepRejectionReason.InvalidQuantities);

        // 7️⃣ Ejecutar TODO lo válido en transacción
        await _uow.BeginAsync(ct);
        try
        {
            var execution = WipStepExecution.CreateNew(
                wip.Id,
                routeStep.Id,
                request.UserId,
                request.DeviceId,
                request.LocationId,
                request.QtyIn,
                request.QtyScrap,
                _clock.UtcNow);

            await _executionRepo.AddAsync(execution, ct);

            // 8️⃣ Avanzar WIP
            var nextStep = await _routeStepRepo.GetNextStepAsync(
                routeStep.RouteId,
                routeStep.StepNumber,
                ct);

            bool woFinished = false;

            if (nextStep is null)
            {
                // Último step
                wip.Finish();

                var wo = await _workOrderRepo.GetByWipIdAsync(wip.Id, ct);
                if (wo is not null)
                {
                    wo.Finish();
                    await _workOrderRepo.UpdateAsync(wo, ct);
                    woFinished = true;
                }
            }
            else
            {
                wip.MoveToStep(nextStep.Id);
            }

            await _wipRepo.UpdateAsync(wip, ct);

            await _uow.CommitAsync(ct);

            return new ExecuteStepResponse
            {
                Success = true,
                Message = "Step ejecutado correctamente",
                NewCurrentStepId = nextStep?.Id,
                WorkOrderFinished = woFinished
            };
        }
        catch
        {
            await _uow.RollbackAsync(ct);
            return Reject("Error interno al ejecutar el step",
                ExecuteStepRejectionReason.InternalError);
        }
    }

    private static ExecuteStepResponse Reject(
        string message,
        ExecuteStepRejectionReason reason)
        => new()
        {
            Success = false,
            Message = $"{message} ({reason})"
        };
}
