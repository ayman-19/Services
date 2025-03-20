using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Domain.Enums;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Queries.GetWorkersOnService
{
    public sealed record GetWorkersOnServiceHandler
        : IRequestHandler<GetWorkersOnServiceQuery, ResponseOf<GetWorkersOnServiceResult>>
    {
        private readonly IServiceRepository _ServiceRepository;

        public GetWorkersOnServiceHandler(IServiceRepository ServiceRepository) =>
            _ServiceRepository = ServiceRepository;

        public async Task<ResponseOf<GetWorkersOnServiceResult>> Handle(
            GetWorkersOnServiceQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var workersService = await _ServiceRepository.GetAsync(
                    s =>
                        s.Id == request.ServiceId
                        && s.WorkerServices.Any(ws => ws.Worker.Status == Status.Active),
                    s => new GetWorkersOnServiceResult(
                        s.Id,
                        s.Name,
                        s.WorkerServices.Where(ws =>
                                (request.WorkerId == null || ws.WorkerId == request.WorkerId)
                                && ws.Worker.Status == Status.Active
                            )
                            .Select(ws => new GetWorkerResult(
                                ws.WorkerId,
                                ws.Worker.User.Name,
                                ws.BranchId,
                                ws.Branch.Name
                            ))
                            .ToList()
                    ),
                    include =>
                        include
                            .Include(s => s.WorkerServices)
                            .ThenInclude(ws => ws.Worker)
                            .ThenInclude(w => w.User)
                            .Include(s => s.WorkerServices)
                            .ThenInclude(b => b.Branch),
                    false,
                    cancellationToken
                );
                return new()
                {
                    Message = ValidationMessages.Success,
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = workersService,
                };
            }
            catch
            {
                throw new DatabaseTransactionException(ValidationMessages.Database.Error);
            }
        }
    }
}
