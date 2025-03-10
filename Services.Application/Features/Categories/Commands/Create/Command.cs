using MediatR;
using Services.Domain.Entities;
using Services.Shared.Responses;

namespace Services.Application.Features.Categories.Commands.Create
{
    public sealed record CreateCategoryCommand(string Name)
        : IRequest<ResponseOf<CreateCategoryResult>>
    {
        public Guid ParentId { get; set; } = Guid.Empty;

        public static implicit operator Category(CreateCategoryCommand categoryCommand) =>
            new()
            {
                ParentId = categoryCommand.ParentId == Guid.Empty ? null : categoryCommand.ParentId,
                Name = categoryCommand.Name,
            };
    }
}
