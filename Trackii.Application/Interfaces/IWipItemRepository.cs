using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Entities;

namespace Trackii.Application.Interfaces;

public interface IWipItemRepository
{
    Task<WipItem?> GetByIdAsync(uint id, CancellationToken ct = default);
    Task AddAsync(WipItem wipItem, CancellationToken ct = default);
    Task UpdateAsync(WipItem wipItem, CancellationToken ct = default);
}