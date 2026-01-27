using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackii.Domain.Entities;

namespace Trackii.Application.Interfaces;

public interface IScanEventRepository
{
    Task AddAsync(ScanEvent scanEvent, CancellationToken ct = default);
}