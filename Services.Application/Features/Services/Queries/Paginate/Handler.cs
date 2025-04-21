using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http;
using Services.Application.Abstarction;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Services.Queries.Paginate
{
    public sealed class PaginateServiceHandler(
        IFileService _fileService,
        IServiceRepository _serviceRepository
    )
        : IRequestHandler<
            PaginateServiceQuery,
            ResponseOf<IReadOnlyCollection<PaginateServiceResult>>
        >
    {
        public async Task<ResponseOf<IReadOnlyCollection<PaginateServiceResult>>> Handle(
            PaginateServiceQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                IReadOnlyCollection<PaginateServiceResult>? result =
                    await _serviceRepository.PaginateAsync(
                        request.page == 0 ? 1 : request.page,
                        request.pageSize == 0 ? 10 : request.pageSize,
                        s => new PaginateServiceResult(
                            s.Id,
                            s.Name,
                            s.Description,
                            _fileService.GetUrlImage(s.Image)
                        ),
                        s =>
                            (s.CategoryId == request.categoryId || request.categoryId == null)
                            && (s.Id == request.Id || request.Id == null),
                        null!,
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
