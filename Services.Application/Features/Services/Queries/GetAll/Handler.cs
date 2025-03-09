using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Services.Queries.GetAll
{
    public sealed record GetAllServicesHandler
        : IRequestHandler<
            GetAllServicesQuery,
            ResponseOf<IReadOnlyCollection<GetAllServicesResult>>
        >
    {
        private readonly IServiceRepository _serviceRepository;

        public GetAllServicesHandler(IServiceRepository serviceRepository) =>
            _serviceRepository = serviceRepository;

        public async Task<ResponseOf<IReadOnlyCollection<GetAllServicesResult>>> Handle(
            GetAllServicesQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                IReadOnlyCollection<GetAllServicesResult>? result =
                    await _serviceRepository.GetAllAsync(
                        s => new GetAllServicesResult(s.Id, s.Name, s.Description),
                        s => s.CategoryId == request.categoryId,
                        null!,
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
