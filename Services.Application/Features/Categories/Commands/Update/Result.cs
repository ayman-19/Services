using Services.Domain.Entities;

namespace Services.Application.Features.Categories.Commands.Update
{
    public sealed record UpdateCategoryResult(Guid? parentid, Guid id, string name)
    {
        public static implicit operator UpdateCategoryResult(Category c) =>
            new(c.ParentId, c.Id, c.Name);
    }
}
