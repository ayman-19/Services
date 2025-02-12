using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Services.Commands.Delete
{
    public sealed class DeleteServiceHandler : IRequestHandler<DeleteServiceCommand, Response>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteServiceHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
        {
            _serviceRepository = serviceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(
            DeleteServiceCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    await _serviceRepository.DeleteByIdAsync(request.Id, cancellationToken);
                    //await _unitOfWork.SaveChangesAsync(cancellationToken);
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
