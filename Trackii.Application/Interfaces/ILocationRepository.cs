using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Entities;

namespace Trackii.Application.Interfaces;

public interface ILocationRepository
{
    Task<Location?> GetByIdAsync(uint id, CancellationToken ct = default);
    Task<bool> HasActiveProductionAsync(uint locationId, CancellationToken ct = default);
    Task AddAsync(Location location, CancellationToken ct = default);
    Task UpdateAsync(Location location, CancellationToken ct = default);
}