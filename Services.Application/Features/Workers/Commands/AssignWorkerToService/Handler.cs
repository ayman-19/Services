using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Commands.AssignWorkerToService
{
    public sealed class AssignWorkerToServiceHandler
        : IRequestHandler<AssignWorkerToServiceCommand, ResponseOf<AssignWorkerToServiceResult>>
    {
        private readonly IWorkerServiceRepository _workerServiceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AssignWorkerToServiceHandler(
            IWorkerServiceRepository workerServiceRepository,
            IUnitOfWork unitOfWork
        )
        {
            _workerServiceRepository = workerServiceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseOf<AssignWorkerToServiceResult>> Handle(
            AssignWorkerToServiceCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    WorkerService workerService = request;
                    await _workerServiceRepository.CreateAsync(workerService, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync();

                    var result = await _workerServiceRepository.GetAsync(
                        w =>
                            w.WorkerId == request.WorkerId
                            && w.ServiceId == request.ServiceId
                            && w.BranchId == request.BranchId,
                        selector => new AssignWorkerToServiceResult(
                            selector.WorkerId,
                            selector.Worker.User.Name,
                            selector.ServiceId,
                            selector.Service.Name,
                            selector.BranchId,
                            selector.Branch.Name
                        ),
                        include =>
                            include
                                .Include(ws => ws.Branch)
                                .Include(ws => ws.Service)
                                .Include(ws => ws.Worker)
                                .ThenInclude(w => w.User),
                        false,
                        cancellationToken
                    );

                    return new ResponseOf<AssignWorkerToServiceResult>
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = result,
                    };
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw new DatabaseTransactionException(ValidationMessages.Database.Error);
                }
            }
        }
    }
}
