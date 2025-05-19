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
                .WithMessage(ValidationMessages.Categories.NameIsRequired)
                .NotEmpty()
                .WithMessage(ValidationMessages.Categories.NameIsRequired);

            RuleFor(c => c.Name)
                .MustAsync(
                    async (name, CancellationToken) =>
                        !await categoryRepository.IsAnyExistAsync(c => c.Name == name)
                )
                .WithMessage(ValidationMessages.Categories.CategoryExists);

            RuleFor(c => c.ParentId)
                .MustAsync(
                    async (id, CancellationToken) =>
                        id == null
                            ? true
                            : await categoryRepository.IsAnyExistAsync(c => c.Id == id)
                )
                .WithMessage(ValidationMessages.Categories.CategoryDoesNotExist);
        }
    }
}
