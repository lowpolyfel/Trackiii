using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Trackii.Domain.Common;

public static class Guard
{
    public static string NotNullOrWhiteSpace(string? value, string paramName, int? maxLen = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException($"{paramName} no puede ser vacío.");

        var trimmed = value.Trim();

        if (maxLen.HasValue && trimmed.Length > maxLen.Value)
            throw new DomainException($"{paramName} no puede exceder {maxLen.Value} caracteres.");

        return trimmed;
    }
}
