using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Categories.Commands.Update
{
    public sealed record UpdateCategoryCommand(Guid Id, string Name)
        : IRequest<ResponseOf<UpdateCategoryResult>>
    {
        public Guid ParentId { get; set; } = Guid.Empty;
    }
}
