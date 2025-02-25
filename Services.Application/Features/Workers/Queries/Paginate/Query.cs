using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Services.Application.Features.Services.Queries.Paginate;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.Paginate
{
    public sealed record PaginateWorkerServiceQuery(int page, int pageSize)
        : IRequest<ResponseOf<IReadOnlyCollection<PaginateWorkerServiceResult>>>;
}
