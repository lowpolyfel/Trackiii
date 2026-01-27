using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Entities;

namespace Trackii.Application.Interfaces;

public interface ISubfamilyRepository
{
    Task<Subfamily?> GetByIdAsync(uint id, CancellationToken ct = default);
    Task<bool> HasActiveProductionAsync(uint subfamilyId, CancellationToken ct = default);
    Task AddAsync(Subfamily subfamily, CancellationToken ct = default);
    Task UpdateAsync(Subfamily subfamily, CancellationToken ct = default);
}