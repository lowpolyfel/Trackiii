using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Entities;

namespace Trackii.Application.Interfaces;

public interface IDeviceRepository
{
    Task<Device?> GetByIdAsync(uint id, CancellationToken ct = default);
    Task<Device?> GetByUidAsync(string deviceUid, CancellationToken ct = default);
    Task AddAsync(Device device, CancellationToken ct = default);
    Task UpdateAsync(Device device, CancellationToken ct = default);
}