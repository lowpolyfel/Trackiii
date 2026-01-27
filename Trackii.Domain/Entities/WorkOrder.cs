using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Common;
using Trackii.Domain.Enums;

namespace Trackii.Domain.Entities;

/// <summary>
/// WorkOrder: orden formal de producción.
/// Reglas de dominio:
/// - Pertenece a un Product.
/// - Se crea en OPEN.
/// - Pasa a IN_PROGRESS al entrar al primer step.
/// - FINISHED solo en el último step.
/// - CANCELLED puede ocurrir en cualquier punto.
/// - No se reabre una WO finalizada o cancelada.
/// - En principio genera 1 WIP (modelo abierto a futuro).
/// </summary>
public sealed class WorkOrder
{
    public uint Id { get; private set; }
    public string WoNumber { get; private set; } = string.Empty;
    public uint ProductId { get; private set; }
    public WorkOrderStatus Status { get; private set; }

    // Constructor para rehidratación (BD)
    public WorkOrder(
        uint id,
        string woNumber,
        uint productId,
        WorkOrderStatus status = WorkOrderStatus.OPEN)
    {
        if (productId == 0)
            throw new DomainException("WorkOrder debe pertenecer a un Product válido.");

        Id = id;
        WoNumber = Guard.NotNullOrWhiteSpace(woNumber, nameof(woNumber), maxLen: 50);
        ProductId = productId;
        Status = status;
    }

    // Factory para creación nueva (Id lo asigna BD)
    public static WorkOrder CreateNew(string woNumber, uint productId)
        => new(0, woNumber, productId, WorkOrderStatus.OPEN);

    /// <summary>
    /// Marca la WO como en progreso (primer ingreso a la línea).
    /// </summary>
    public void Start()
    {
        if (Status != WorkOrderStatus.OPEN)
            throw new DomainException("Solo una WorkOrder en estado OPEN puede iniciar.");

        Status = WorkOrderStatus.IN_PROGRESS;
    }

    /// <summary>
    /// Marca la WO como finalizada.
    /// Debe validarse externamente que el WIP esté en el último step.
    /// </summary>
    public void Finish()
    {
        if (Status != WorkOrderStatus.IN_PROGRESS)
            throw new DomainException("Solo una WorkOrder en progreso puede finalizar.");

        Status = WorkOrderStatus.FINISHED;
    }

    /// <summary>
    /// Cancela la WO en cualquier punto antes de finalizar.
    /// </summary>
    public void Cancel()
    {
        if (Status == WorkOrderStatus.FINISHED)
            throw new DomainException("No se puede cancelar una WorkOrder finalizada.");

        if (Status == WorkOrderStatus.CANCELLED)
            return;

        Status = WorkOrderStatus.CANCELLED;
    }

    public override string ToString()
        => $"WorkOrder(Id={Id}, WoNumber={WoNumber}, ProductId={ProductId}, Status={Status})";
}
