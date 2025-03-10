using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Categories.Commands.Create
{
    public sealed class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public CreateCategoryValidator(IServiceProvider serviceProvider)
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
            RuleFor(c => c.Name)
                .NotNull()
                .WithMessage(ValidationMessages.Category.NameIsRequired)
                .NotEmpty()
                .WithMessage(ValidationMessages.Category.NameIsRequired);

            RuleFor(c => c.ParentId)
                .MustAsync(
                    async (id, CancellationToken) =>
                        id == Guid.Empty
                            ? true
                            : await categoryRepository.IsAnyExistAsync(c => c.Id == id)
                )
                .WithMessage(ValidationMessages.Category.CategoryNotExist);
        }
    }
}
