using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Commands.UpdateWorkerStatus
{
    public sealed class UpdateWorkerStatusHandler
        : IRequestHandler<UpdateWorkerStatusCommand, Response>
    {
        private readonly IWorkerRepository _workerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWorkerStatusHandler(IWorkerRepository workerRepository, IUnitOfWork unitOfWork)
        {
            _workerRepository = workerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(
            UpdateWorkerStatusCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    await _workerRepository.UpdateStatusAsync(request.WorkerId, request.Status);
                    await transaction.CommitAsync(cancellationToken);
                    return new()
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new Exception(ex.Message, ex);
                }
            }
        }
    }
}
