using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Entities;

namespace Trackii.Application.Interfaces;

public interface ITokenRepository
{
    Task<Token?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task AddAsync(Token token, CancellationToken ct = default);
    Task UpdateAsync(Token token, CancellationToken ct = default);
}