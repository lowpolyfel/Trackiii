using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Common;
using Trackii.Domain.Enums;

namespace Trackii.Domain.Entities;

/// <summary>
/// WipItem: pieza o lote físico que se mueve por la fábrica.
/// Reglas de dominio:
/// - Pertenece a una WorkOrder.
/// - Siempre tiene un Step actual.
/// - Representa el ESTADO ACTUAL (no historial).
/// - Puede representar desde 1 hasta miles de piezas (qty_input).
/// - El avance es estrictamente secuencial (no se permiten saltos).
/// - El cambio de step ocurre como resultado de una ejecución válida.
/// </summary>
public sealed class WipItem
{
    public uint Id { get; private set; }
    public uint WorkOrderId { get; private set; }
    public uint CurrentStepId { get; private set; }
    public WipStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public uint QtyInput { get; private set; }

    // Constructor para rehidratación (BD)
    public WipItem(
        uint id,
        uint workOrderId,
        uint currentStepId,
        DateTime createdAt,
        uint qtyInput,
        WipStatus status = WipStatus.ACTIVE)
    {
        if (workOrderId == 0)
            throw new DomainException("WipItem debe pertenecer a una WorkOrder válida.");

        if (currentStepId == 0)
            throw new DomainException("WipItem debe tener un Step inicial válido.");

        if (qtyInput == 0)
            throw new DomainException("QtyInput debe ser mayor a cero.");

        Id = id;
        WorkOrderId = workOrderId;
        CurrentStepId = currentStepId;
        CreatedAt = createdAt;
        QtyInput = qtyInput;
        Status = status;
    }

    // Factory para creación nueva (Id lo asigna BD)
    public static WipItem CreateNew(
        uint workOrderId,
        uint firstStepId,
        uint qtyInput,
        DateTime createdAt)
        => new(
            0,
            workOrderId,
            firstStepId,
            createdAt,
            qtyInput,
            WipStatus.ACTIVE);

    /// <summary>
    /// Avanza el WIP al siguiente paso.
    /// La validación de que sea el step correcto (secuencia)
    /// se realiza fuera de esta entidad.
    /// </summary>
    public void MoveToStep(uint nextStepId)
    {
        if (Status != WipStatus.ACTIVE)
            throw new DomainException("Solo un WIP activo puede avanzar de paso.");

        if (nextStepId == 0)
            throw new DomainException("Step inválido.");

        CurrentStepId = nextStepId;
    }

    public void Hold()
    {
        if (Status != WipStatus.ACTIVE)
            throw new DomainException("Solo un WIP activo puede ponerse en HOLD.");

        Status = WipStatus.HOLD;
    }

    public void Resume()
    {
        if (Status != WipStatus.HOLD)
            throw new DomainException("Solo un WIP en HOLD puede reanudarse.");

        Status = WipStatus.ACTIVE;
    }

    public void Finish()
    {
        if (Status != WipStatus.ACTIVE)
            throw new DomainException("Solo un WIP activo puede finalizar.");

        Status = WipStatus.FINISHED;
    }

    public void Scrap()
    {
        if (Status == WipStatus.FINISHED)
            throw new DomainException("No se puede scrapear un WIP finalizado.");

        Status = WipStatus.SCRAPPED;
    }

    public override string ToString()
        => $"WipItem(Id={Id}, WorkOrderId={WorkOrderId}, Step={CurrentStepId}, Qty={QtyInput}, Status={Status})";
}
