using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Entities;

namespace Trackii.Application.Interfaces;

public interface IRouteStepRepository
{
    Task<RouteStep?> GetByIdAsync(uint id, CancellationToken ct = default);
    Task<RouteStep?> GetFirstStepAsync(uint routeId, CancellationToken ct = default);
    Task<RouteStep?> GetNextStepAsync(uint routeId, uint currentStepNumber, CancellationToken ct = default);
}