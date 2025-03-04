using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Domain.Enums;

namespace Services.Application.Features.Workers.Queries.GetWorkersBasedOnStatus
{
    public sealed record GetWorkerResultPaginate(Guid WorkerId, string WorkerName, Status status);
}
