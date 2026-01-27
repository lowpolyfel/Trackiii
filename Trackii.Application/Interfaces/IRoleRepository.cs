using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Entities;

namespace Trackii.Application.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(uint id, CancellationToken ct = default);
    Task AddAsync(Role role, CancellationToken ct = default);
}
