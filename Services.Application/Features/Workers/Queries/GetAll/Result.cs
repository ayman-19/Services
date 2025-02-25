using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Features.Workers.Queries.GetAll
{
    public sealed record GetAllWorkerResult(
        Guid WorkerServiceId,
        Guid ServiceId,
        string ServiceName,
        Guid WorkerId,
        string WorkerName,
        Guid BranchId,
        string BranchName,
        bool Availabilty
    );
}
