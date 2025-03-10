using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Categories.Commands.Delete
{
    public sealed class DeleteCategoryValidator : AbstractValidator<DeleteCategoryCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteCategoryValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<ICategoryRepository>());
        }

        private void ValidateRequest(ICategoryRepository categoryRepository)
        {
            RuleFor(category => category.id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Category.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Category.IdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await categoryRepository.IsAnyExistAsync(category => category.Id == id)
                )
                .WithMessage(ValidationMessages.Category.CategoryNotExist);
        }
    }
}
