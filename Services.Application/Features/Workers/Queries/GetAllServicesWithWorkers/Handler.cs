using System.Linq;
using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Application.Features.Services.Queries.GetById;
using Services.Domain.Abstraction;
using Services.Domain.Enums;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Queries.GetAllServicesWithWorkers
{
    public sealed record GetAllServicesWithWorkersHandler
        : IRequestHandler<
            GetAllServicesWithWorkersQuery,
            ResponseOf<GetAllServicesWithWorkersResult>
        >
    {
        private readonly IWorkerRepository _workerRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public GetAllServicesWithWorkersHandler(
            IWorkerRepository workerRepository,
            IHttpContextAccessor contextAccessor
        )
        {
            _workerRepository = workerRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<ResponseOf<GetAllServicesWithWorkersResult>> Handle(
            GetAllServicesWithWorkersQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var result = await _workerRepository.GetAsync(
                    ws => ws.UserId == request.WorkerId && ws.Status == Status.Active,
                    ws => new GetAllServicesWithWorkersResult(
                        ws.UserId,
                        ws.User.Name,
                        ws.WorkerServices.Where(s =>
                                s.ServiceId == request.ServiceId || request.ServiceId == null
                            )
                            .Select(s => new GetServiceResult(
                                s.Service.Id,
                                s.Service.Name,
                                s.Service.Description,
                                GetUrlImage(s.Service.Image)
                            ))
                            .ToList()
                    ),
                    ws =>
                        ws.Include(w => w.WorkerServices)
                            .ThenInclude(ws => ws.Service)
                            .Include(w => w.User),
                    false,
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

        private string GetUrlImage(string fileName) =>
            $"{_contextAccessor.HttpContext!.Request.Scheme}://{_contextAccessor.HttpContext!.Request.Host}{_contextAccessor.HttpContext!.Request.PathBase}"
            + fileName;
    }
}
