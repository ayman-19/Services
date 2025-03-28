using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Abstraction;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Categories.Queries.PaginateParentCategories
{
    public sealed class PaginateParentCategoriesHandler(ICategoryRepository _categoryRepository)
        : IRequestHandler<PaginateCategoriesQuery, ResponseOf<PaginateCategoriesResults>>
    {
        public async Task<ResponseOf<PaginateCategoriesResults>> Handle(
            PaginateCategoriesQuery request,
            CancellationToken cancellationToken
        )
        {
            int page = request.page == 0 ? 1 : request.page;
            int pagesize = request.pageSize == 0 ? 10 : request.pageSize;
            var categories = await _categoryRepository.GetAllAsync(c => new CategoriesFliter(
                c.Id,
                c.Name,
                c.ParentId
            ));

            var categoriesHierarchy = GetAllCategoriesHierarchy(categories, request.Id);

            var result = categoriesHierarchy
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pagesize)
                .Take(pagesize);

            return new()
            {
                Message = ValidationMessages.Success,
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Result = new(
                    page,
                    pagesize,
                    (int)Math.Ceiling(result.Count() / (double)pagesize),
                    result
                ),
            };
        }

        public IQueryable<CategoriesResult> GetAllCategoriesHierarchy(
            IReadOnlyCollection<CategoriesFliter> categories,
            Guid? id
        ) =>
            categories
                .Where(c => c.ParentId == null && (c.Id == id || id == null))
                .Select(c => new CategoriesResult(
                    c.Id,
                    c.Name,
                    c.ParentId,
                    GetCategoryHierarchy(categories, c.Id)
                ))
                .AsQueryable();

        public IQueryable<CategoriesResult> GetCategoryHierarchy(
            IReadOnlyCollection<CategoriesFliter> categories,
            Guid? parentId
        )
        {
            return categories
                .Where(c => c.ParentId == parentId)
                .Select(c => new CategoriesResult(
                    c.Id,
                    c.Name,
                    c.ParentId,
                    GetCategoryHierarchy(categories, c.Id)
                ))
                .AsQueryable();
        }
    }
}
