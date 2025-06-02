using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Categories.Queries.GetAll
{
    public sealed class GetAllCategoriesHandler(ICategoryRepository _categoryRepository)
        : IRequestHandler<
            GetAllCategoriesQuery,
            ResponseOf<IReadOnlyCollection<GetAllCategoriesResult>>
        >
    {
        public async Task<ResponseOf<IReadOnlyCollection<GetAllCategoriesResult>>> Handle(
            GetAllCategoriesQuery request,
            CancellationToken cancellationToken
        )
        {
            var categories = await _categoryRepository.GetAllAsync(
                c => new GetAllCategoriesResult(c.Id, c.Name),
                c =>
                    c.Name.Contains(request.searchName)
                    || string.IsNullOrWhiteSpace(request.searchName),
                cancellationToken: cancellationToken
            );

            return new()
            {
                Message = ValidationMessages.Success,
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Result = categories,
            };
        }
    }
}
