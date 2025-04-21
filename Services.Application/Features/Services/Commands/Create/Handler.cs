using System.Net;
using MediatR;
using Services.Application.Abstarction;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Services.Commands.Create
{
    public sealed class CreateServiceHandler
        : IRequestHandler<CreateServiceCommand, ResponseOf<CreateServiceResult>>
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public CreateServiceHandler(
            IServiceRepository serviceRepository,
            IUnitOfWork unitOfWork,
            IFileService fileService
        )
        {
            _serviceRepository = serviceRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<ResponseOf<CreateServiceResult>> Handle(
            CreateServiceCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    Service service = request;
                    service.Image = await _fileService.SaveImageAsync(
                        request.File.OpenReadStream(),
                        request.File.FileName
                    );
                    await _serviceRepository.CreateAsync(service, cancellationToken);
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
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new DatabaseTransactionException(ex.Message);
                }
            }
        }
    }
}
