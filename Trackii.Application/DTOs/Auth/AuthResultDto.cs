using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackii.Application.Dtos.Auth;

public sealed class AuthResultDto
{
    public uint UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string RoleName { get; init; } = string.Empty;
}
