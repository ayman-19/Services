using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Categories.Queries.GetAll
{
    public sealed record GetAllCategoriesQuery(string searchName)
        : IRequest<ResponseOf<IReadOnlyCollection<GetAllCategoriesResult>>>;
}
