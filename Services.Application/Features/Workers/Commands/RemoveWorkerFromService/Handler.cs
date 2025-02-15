using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Commands.RemoveWorkerFromService
{
    public sealed class RemoveWorkerFromServiceHandler
        : IRequestHandler<RemoveWorkerFromServiceCommand, Response>
    {
        private readonly IWorkerServiceRepository _workerServiceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveWorkerFromServiceHandler(
            IWorkerServiceRepository workerServiceRepository,
            IUnitOfWork unitOfWork
        )
        {
            _workerServiceRepository = workerServiceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(
            RemoveWorkerFromServiceCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    await _workerServiceRepository.DeleteWorkerFromServiceAsync(
                        request.WorkerId,
                        request.ServiceId,
                        cancellationToken
                    );
                    await transaction.CommitAsync();
                    return new()
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
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
