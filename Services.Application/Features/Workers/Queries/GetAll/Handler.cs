using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Queries.GetAll
{
    public sealed record GettAllWorkerHandler
        : IRequestHandler<GetAllWorkerQuery, ResponseOf<IReadOnlyCollection<GetAllWorkerResult>>>
    {
        private readonly IWorkerServiceRepository _workerserviceRepository;

        public GettAllWorkerHandler(IWorkerServiceRepository workerserviceRepository) =>
            _workerserviceRepository = workerserviceRepository;

        public async Task<ResponseOf<IReadOnlyCollection<GetAllWorkerResult>>> Handle(
            GetAllWorkerQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                IReadOnlyCollection<GetAllWorkerResult>? result =
                    await _workerserviceRepository.GetAllAsync(
                        ws => new GetAllWorkerResult(
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
                    Result = result,
                };
            }
            catch
            {
                throw new DatabaseTransactionException(ValidationMessages.Database.Error);
            }
        }
    }
}
