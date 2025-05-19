using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Categories.Commands.Update
{
    public sealed class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdateCategoryValidator(IServiceProvider serviceProvider)
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
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Categories.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Categories.IdIsRequired);

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.Categories.NameIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Categories.NameIsRequired);

            RuleFor(b => b.Id)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await categoryRepository.IsAnyExistAsync(c => c.Id == id)
                )
                .WithMessage(ValidationMessages.Categories.CategoryDoesNotExist);

            RuleFor(c => c.ParentId)
                .MustAsync(
                    async (id, CancellationToken) =>
                        id == null
                            ? true
                            : await categoryRepository.IsAnyExistAsync(c => c.Id == id)
                )
                .WithMessage(ValidationMessages.Categories.CategoryDoesNotExist);

            RuleFor(b => b)
                .MustAsync(
                    async (request, CancellationToken) =>
                        !await categoryRepository.IsAnyExistAsync(n =>
                            n.Name == request.Name && n.Id != request.Id
                        )
                )
                .WithMessage(ValidationMessages.Categories.CategoryExists);
        }
    }
}
