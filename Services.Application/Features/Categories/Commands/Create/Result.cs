using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Domain.Entities;

namespace Services.Application.Features.Categories.Commands.Create
{
    public sealed record CreateCategoryResult(Guid? parentid, Guid categoryid, string name)
    {
        public static implicit operator CreateCategoryResult(Category category) =>
            new(category.ParentId, category.Id, category.Name);
    }
}
