using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Application.Features.Workers.Queries.Paginate;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
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
            int page = request.page == 0 ? 1 : request.page;
            int pagesize = request.pagesize == 0 ? 1 : request.pagesize;

            IReadOnlyCollection<GetAllWorkerPaginateResult>? result =
                await _WorkerserviceRepository.PaginateAsync(
                    page,
                    pagesize,
                    ws => new GetAllWorkerPaginateResult(
                        ws.Id,
                        ws.Service.Id,
                        ws.Service.Name,
                        ws.Worker.UserId,
                        ws.Worker.User.Name,
                        ws.Branch.Id,
                        ws.Branch.Name,
                        ws.Availabilty
                    ),
                    null!,
                    q =>
                        q.Include(ws => ws.Service)
                            .Include(ws => ws.Worker)
                            .ThenInclude(w => w.User)
                            .Include(ws => ws.Branch),
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
                    (int)Math.Ceiling(result.Count / (double)pagesize),
                    result
                ),
            };
        }
    }
}
