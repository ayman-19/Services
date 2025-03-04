using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Services.Domain.Enums;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetWorkersBasedOnStatus
{
    public sealed record GetWorkerQueryPaginate(Status status, int page, int pagesize)
        : IRequest<ResponseOf<IReadOnlyCollection<GetWorkerResultPaginate>>>;
}
