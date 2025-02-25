using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Queries.Paginate
{
    public sealed class PaginateWorkerServiceHandler
        : IRequestHandler<
            PaginateWorkerServiceQuery,
            ResponseOf<IReadOnlyCollection<PaginateWorkerServiceResult>>
        >
    {
        private readonly IWorkerServiceRepository _workerServiceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaginateWorkerServiceHandler(
            IWorkerServiceRepository workerServiceRepository,
            IUnitOfWork unitOfWork
        )
        {
            _workerServiceRepository = workerServiceRepository;

            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseOf<IReadOnlyCollection<PaginateWorkerServiceResult>>> Handle(
            PaginateWorkerServiceQuery request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    IReadOnlyCollection<PaginateWorkerServiceResult>? result =
                        await _workerServiceRepository.PaginateAsync(
                            request.page == 0 ? 1 : request.page,
                            request.pageSize == 0 ? 10 : request.pageSize,
                            ws => new PaginateWorkerServiceResult(
                                ws.Id,
                                ws.ServiceId,
                                ws.Service.Name,
                                ws.WorkerId,
                                ws.Worker.User.Name,
                                ws.BranchId,
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
}
