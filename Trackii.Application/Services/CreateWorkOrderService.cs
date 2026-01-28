using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Application.DTOs.WorkOrder;
using Trackii.Application.Interfaces;
using Trackii.Domain.Entities;

namespace Trackii.Application.Services;

public sealed class CreateWorkOrderService
{
    private readonly IProductRepository _productRepo;
    private readonly IWorkOrderRepository _woRepo;
    private readonly IUnitOfWork _uow;

    public CreateWorkOrderService(
        IProductRepository productRepo,
        IWorkOrderRepository woRepo,
        IUnitOfWork uow)
    {
        _productRepo = productRepo;
        _woRepo = woRepo;
        _uow = uow;
    }

    public async Task<CreateWorkOrderResponse> ExecuteAsync(
        CreateWorkOrderRequest request,
        CancellationToken ct = default)
    {
        var product = await _productRepo.GetByIdAsync(request.ProductId, ct)
            ?? throw new InvalidOperationException("Producto no existe.");

        if (!product.Active)
            throw new InvalidOperationException("Producto inactivo.");

        var wo = WorkOrder.CreateNew(request.WoNumber, request.ProductId);

        await _uow.BeginAsync(ct);
        try
        {
            await _woRepo.AddAsync(wo, ct);
            await _uow.CommitAsync(ct);
        }
        catch
        {
            await _uow.RollbackAsync(ct);
            throw;
        }

        return new CreateWorkOrderResponse { WorkOrderId = wo.Id };
    }
}
