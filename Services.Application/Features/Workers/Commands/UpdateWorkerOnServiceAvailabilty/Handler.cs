using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Commands.UpdateWorkerOnServiceAvailabilty
{
    public sealed class UpdateWorkerOnServiceAvailabiltyHandler
        : IRequestHandler<UpdateWorkerOnServiceAvailabiltyCommand, Response>
    {
        private readonly IWorkerServiceRepository _workerServiceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWorkerOnServiceAvailabiltyHandler(
            IWorkerServiceRepository workerServiceRepository,
            IUnitOfWork unitOfWork
        )
        {
            _workerServiceRepository = workerServiceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(
            UpdateWorkerOnServiceAvailabiltyCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    await _workerServiceRepository.UpdateAvailabiltyAsync(
                        request.WorkerId,
                        request.Availabilty,
                        cancellationToken
                    );
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
