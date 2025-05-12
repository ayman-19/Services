using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Categories.Queries.GetAll
{
    public sealed record GetAllCategoriesQuery(Guid? Id)
        : IRequest<ResponseOf<IReadOnlyCollection<GetAllCategoriesResult>>>;
}
