using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Application.Dtos.Auth;

namespace Trackii.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto> LoginAsync(
        string username,
        string password,
        CancellationToken ct = default);
}
