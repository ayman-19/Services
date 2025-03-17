using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Services.Queries.GetById
{
    public sealed class GetServiceHandler
        : IRequestHandler<GetServiceQuery, ResponseOf<GetServiceResult>>
    {
        private readonly IServiceRepository _serviceRepository;

        public GetServiceHandler(IServiceRepository serviceRepository) =>
            _serviceRepository = serviceRepository;

        public async Task<ResponseOf<GetServiceResult>> Handle(
            GetServiceQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var result = await _serviceRepository.GetAsync(
                    s => s.Id == request.Id,
                    s => new GetServiceResult(s.Id, s.Name, s.Description),
                    null!,
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
    }
}
