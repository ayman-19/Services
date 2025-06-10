using System.Linq.Expressions;
using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Services.Application.Features.Workers.Queries.Paginate;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Queries.GetAll
{
    public sealed record GetAllWorkerHandler
        : IRequestHandler<GetWorkerPaginateQuery, ResponseOf<GetWorkerPaginateResult>>
    {
        private readonly IWorkerServiceRepository _WorkerserviceRepository;

        public GetAllWorkerHandler(IWorkerServiceRepository workerserviceRepository) =>
            _WorkerserviceRepository = workerserviceRepository;

        public async Task<ResponseOf<GetWorkerPaginateResult>> Handle(
            GetWorkerPaginateQuery request,
            CancellationToken cancellationToken
        )
        {
            int page = request.page <= 0 ? 1 : request.page;
            int pagesize = request.pagesize <= 0 ? 10 : request.pagesize;

            Expression<Func<WorkerService, GetAllWorkerPaginateResult>> selector =
                ws => new GetAllWorkerPaginateResult(
                    ws.Id,
                    ws.Service.Id,
                    ws.Service.Name,
                    ws.Worker.UserId,
                    ws.Worker.User.Name,
                    ws.Worker.User.Branch.Id,
                    ws.Availabilty,
                    ws.Worker.Status
                );

            Expression<Func<WorkerService, bool>> predicate = w =>
                (
                    string.IsNullOrWhiteSpace(request.searchName)
                    || w.Worker.User.Name.Contains(request.searchName)
                ) && (request.ServiceId == null || w.ServiceId == request.ServiceId);

            Func<IQueryable<WorkerService>, IIncludableQueryable<WorkerService, object>> includes =
                q =>
                    q.Include(ws => ws.Service)
                        .Include(ws => ws.Worker)
                        .ThenInclude(w => w.User)
                        .ThenInclude(u => u.Branch);

            var result = await _WorkerserviceRepository.PaginateAsync(
                page,
                pagesize,
                selector,
                predicate,
                includes,
                ordering: null!,
                cancellationToken
            );

            return new()
            {
                Message = ValidationMessages.Success,
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Result = new(
                    page,
                    pagesize,
                    (int)Math.Ceiling(result.count / (double)pagesize),
                    result.Item1
                ),
            };
        }
    }
}
