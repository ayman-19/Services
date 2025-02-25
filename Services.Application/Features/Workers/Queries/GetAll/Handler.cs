using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Application.Features.Services.Queries.GetAll;
using Services.Application.Features.Workers.Queries.GetAllServicesWithWorkers;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Queries.GetAll
{
    public sealed record GetAllWorkerHandler
        : IRequestHandler<GetAllWorkerQuery, ResponseOf<IReadOnlyCollection<GetAllWorkerResult>>>
    {
        private readonly IWorkerServiceRepository _workerserviceRepository;

        public GetAllWorkerHandler(IWorkerServiceRepository workerserviceRepository) =>
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
