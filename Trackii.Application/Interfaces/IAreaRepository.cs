using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Entities;

namespace Trackii.Application.Interfaces;

public interface IAreaRepository
{
    Task<Area?> GetByIdAsync(uint id, CancellationToken ct = default);
    Task<bool> HasActiveProductionAsync(uint areaId, CancellationToken ct = default);
    Task AddAsync(Area area, CancellationToken ct = default);
    Task UpdateAsync(Area area, CancellationToken ct = default);
}