using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Entities;

namespace Trackii.Application.Interfaces;

public interface IWipStepExecutionRepository
{
    Task<bool> ExistsForStepAsync(uint wipItemId, uint routeStepId, CancellationToken ct = default);
    Task<WipStepExecution?> GetLastExecutionAsync(uint wipItemId, CancellationToken ct = default);
    Task AddAsync(WipStepExecution execution, CancellationToken ct = default);
}
