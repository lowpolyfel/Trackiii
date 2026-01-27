using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Entities;

namespace Trackii.Application.Interfaces;

public interface IFamilyRepository
{
    Task<Family?> GetByIdAsync(uint id, CancellationToken ct = default);
    Task<bool> HasActiveProductionAsync(uint familyId, CancellationToken ct = default);
    Task AddAsync(Family family, CancellationToken ct = default);
    Task UpdateAsync(Family family, CancellationToken ct = default);
}