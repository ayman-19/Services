using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Application.Features.Workers.Queries.Paginate;
using Services.Domain.Abstraction;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Categories.Queries.PaginateParentCategories
{
    public sealed class PaginateParentCategoriesHandler(ICategoryRepository _categoryRepository)
        : IRequestHandler<
            PaginateParentCategoriesQuery,
            ResponseOf<PaginateParentCategoriesResults>
        >
    {
        public async Task<ResponseOf<PaginateParentCategoriesResults>> Handle(
            PaginateParentCategoriesQuery request,
            CancellationToken cancellationToken
        )
        {
            int page = request.page == 0 ? 1 : request.page;
            int pagesize = request.pageSize == 0 ? 1 : request.pageSize;

            IReadOnlyCollection<ParentCategories> result = await _categoryRepository.PaginateAsync(
                page,
                pagesize,
                c => new ParentCategories(c.Id, c.Name, c.SubCategories.Count),
                c => c.ParentId == null,
                c => c.Include(sub => sub.SubCategories),
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
                    (int)Math.Ceiling(result.Count / (double)pagesize),
                    result
                ),
            };
        }
    }
}
