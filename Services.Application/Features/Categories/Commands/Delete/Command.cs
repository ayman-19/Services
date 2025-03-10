using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Categories.Commands.Delete
{
    public sealed record DeleteCategoryCommand(Guid id) : IRequest<Response>;
}
