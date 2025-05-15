using System.Net;
using MediatR;
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
    ) : IRequestHandler<PaginateServiceQuery, ResponseOf<PaginateServiceResult>>
    {
        public async Task<ResponseOf<PaginateServiceResult>> Handle(
            PaginateServiceQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                int page = request.page == 0 ? 1 : request.page;
                int pagesize = request.pageSize == 0 ? 10 : request.pageSize;
                var result = await _serviceRepository.PaginateAsync(
                    page,
                    pagesize,
                    s => new ServiceResult(
                        s.Id,
                        s.Name,
                        s.Description,
                        _fileService.GetUrlImage(s.Image)
                    ),
                    s =>
                        (s.CategoryId == request.categoryId || request.categoryId == null)
                        && (request.Id == null || s.Id == request.Id)
                        && (request.searchName == null || s.Name.Contains(request.searchName)),
                    null!,
                    null!,
                    cancellationToken
                );
                return new()
                {
                    Message = ValidationMessages.Success,
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = new(
                        page,
                        pagesize,
                        (int)Math.Ceiling(result.count / (double)pagesize),
                        result.Item1
                    ),
                };
            }
            catch
            {
                throw new DatabaseTransactionException(ValidationMessages.Database.Error);
            }
        }
    }
}
