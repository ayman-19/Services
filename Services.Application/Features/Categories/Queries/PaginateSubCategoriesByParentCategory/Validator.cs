using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Categories.Queries.PaginateSubCategoriesByParentCategory
{
    public sealed class PaginateSubCategoriesByParentCategoryValidator
        : AbstractValidator<PaginateSubCategoriesByParentCategoryQuery>
    {
        private readonly IServiceProvider _serviceProvider;

        public PaginateSubCategoriesByParentCategoryValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<ICategoryRepository>());
        }

        private void ValidateRequest(ICategoryRepository categoryRepository)
        {
            RuleFor(s => s.parentCategory)
                .NotEmpty()
                .WithMessage(ValidationMessages.Category.CategoryIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Category.CategoryIdIsRequired);

            RuleFor(query => query)
                .MustAsync(
                    async (query, CancellationToken) =>
                        await categoryRepository.IsAnyExistAsync(n => n.Id == query.parentCategory)
                )
                .WithMessage(ValidationMessages.Category.CategoryIdIsRequired);
        }
    }
}
