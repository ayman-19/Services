using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Services.Application.Features.Services.Commands.Create;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Services.Commands.Update
{
    public sealed class UpdateServiceHandler
        : IRequestHandler<UpdateServiceCommand, ResponseOf<UpdateServiceResult>>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateServiceHandler(IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
        {
            _serviceRepository = serviceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseOf<UpdateServiceResult>> Handle(
            UpdateServiceCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    Service service = await _serviceRepository.GetByIdAsync(request.id);
                    service.UpdateService(request.name, request.description);
                    await _serviceRepository.UpdateAsync(service, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync();
                    return new()
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = service,
                    };
                }
                catch
                {
                    await transaction.CommitAsync();
                    throw new DatabaseTransactionException(ValidationMessages.Database.Error);
                }
            }
        }
    }
}
