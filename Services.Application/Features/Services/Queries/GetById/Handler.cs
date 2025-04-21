using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http;
using Services.Application.Abstarction;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Services.Queries.GetById
{
    public sealed class GetServiceHandler(
        IFileService _fileService,
        IServiceRepository _serviceRepository
    ) : IRequestHandler<GetServiceQuery, ResponseOf<GetServiceResult>>
    {
        public async Task<ResponseOf<GetServiceResult>> Handle(
            GetServiceQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var result = await _serviceRepository.GetAsync(
                    s => s.Id == request.Id,
                    s => new GetServiceResult(
                        s.Id,
                        s.Name,
                        s.Description,
                        _fileService.GetUrlImage(s.Image)
                    ),
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
