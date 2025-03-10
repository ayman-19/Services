using MediatR;
using Services.Domain.Entities;
using Services.Shared.Responses;

namespace Services.Application.Features.Categories.Commands.Create
{
    public sealed record CreateCategoryCommand(string Name, Guid? ParentId)
        : IRequest<ResponseOf<CreateCategoryResult>>
    {
        public static implicit operator Category(CreateCategoryCommand categoryCommand) =>
            new() { ParentId = categoryCommand.ParentId, Name = categoryCommand.Name };
    }
}
