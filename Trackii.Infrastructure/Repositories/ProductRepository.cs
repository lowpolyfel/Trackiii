using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Ruta: Trackii.Infrastructure/Repositories/DeviceRepository.cs
using Microsoft.EntityFrameworkCore;
using Trackii.Application.Interfaces;
using Trackii.Domain.Entities;
using Trackii.Infrastructure.Persistence;

namespace Trackii.Infrastructure.Repositories;

public sealed class DeviceRepository : IDeviceRepository
{
    private readonly TrackiiDbContext _db;

    public DeviceRepository(TrackiiDbContext db)
    {
        _db = db;
    }

    public async Task<Device?> GetByIdAsync(uint id, CancellationToken ct = default)
    {
        return await _db.Devices
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, ct);
    }

    public async Task<Device?> GetByUidAsync(string deviceUid, CancellationToken ct = default)
    {
        return await _db.Devices
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.DeviceUid == deviceUid && d.Active, ct);
    }

    public async Task AddAsync(Device device, CancellationToken ct = default)
    {
        await _db.Devices.AddAsync(device, ct);
    }

    public Task UpdateAsync(Device device, CancellationToken ct = default)
    {
        _db.Devices.Update(device);
        return Task.CompletedTask;
    }
}
