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

public sealed class ProductRepository : IProductRepository
{
    private readonly TrackiiDbContext _db;

    public ProductRepository(TrackiiDbContext db)
    {
        _db = db;
    }

    public async Task<Product?> GetByIdAsync(uint id, CancellationToken ct = default)
    {
        return await _db.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id && p.Active, ct);
    }

    public async Task<Product?> GetByPartNumberAsync(string partNumber, CancellationToken ct = default)
    {
        return await _db.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PartNumber == partNumber && p.Active, ct);
    }

    public async Task<bool> HasActiveProductionAsync(uint productId, CancellationToken ct = default)
    {
        return await _db.WorkOrders
            .AnyAsync(w =>
                w.ProductId == productId &&
                w.Status != Domain.Enums.WorkOrderStatus.CANCELLED &&
                w.Status != Domain.Enums.WorkOrderStatus.FINISHED,
                ct);
    }

    public async Task AddAsync(Product product, CancellationToken ct = default)
    {
        await _db.Products.AddAsync(product, ct);
    }

    public Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        _db.Products.Update(product);
        return Task.CompletedTask;
    }
}
