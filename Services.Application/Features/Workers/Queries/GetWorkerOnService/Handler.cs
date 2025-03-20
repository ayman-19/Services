using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Domain.Enums;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Queries.GetWorkerOnService
{
    public sealed record GetWorkerOnServiceHandler
        : IRequestHandler<GetWorkerOnServiceQuery, ResponseOf<GetWorkerOnServiceResult>>
    {
        private readonly IWorkerServiceRepository _workerServiceRepository;

        public GetWorkerOnServiceHandler(IWorkerServiceRepository workerServiceRepository) =>
            _workerServiceRepository = workerServiceRepository;

        public async Task<ResponseOf<GetWorkerOnServiceResult>> Handle(
            GetWorkerOnServiceQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                GetWorkerOnServiceResult? workerService = await _workerServiceRepository.GetAsync(
                    n =>
                        n.WorkerId == request.WorkerId
                        && n.ServiceId == request.ServiceId
                        && n.BranchId == request.BranchId
                        && n.Worker.Status == Status.Active,
                    ws => new GetWorkerOnServiceResult(
                        ws.WorkerId,
                        ws.Worker.User.Name,
                        ws.ServiceId,
                        ws.Service.Name,
                        ws.BranchId,
                        ws.Branch.Name
                    ),
                    ws =>
                        ws.Include(w => w.Worker)
                            .ThenInclude(u => u.User)
                            .Include(s => s.Service)
                            .Include(b => b.Branch),
                    false,
                    cancellationToken
                );
                return new()
                {
                    Message = ValidationMessages.Success,
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = workerService,
                };
            }
            catch
            {
                throw new DatabaseTransactionException(ValidationMessages.Database.Error);
            }
        }
    }
}
