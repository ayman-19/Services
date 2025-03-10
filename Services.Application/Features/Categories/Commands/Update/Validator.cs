using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                .WithMessage(ValidationMessages.Category.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Category.IdIsRequired);

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.Category.NameIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Category.NameIsRequired);

            RuleFor(b => b.Id)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await categoryRepository.IsAnyExistAsync(c => c.Id == id)
                )
                .WithMessage(ValidationMessages.Category.CategoryNotExist);

            RuleFor(c => c.ParentId)
                .MustAsync(
                    async (id, CancellationToken) =>
                        id != default
                            ? true
                            : await categoryRepository.IsAnyExistAsync(c => c.Id == id)
                )
                .WithMessage(ValidationMessages.Category.CategoryNotExist);

            RuleFor(b => b)
                .MustAsync(
                    async (request, CancellationToken) =>
                        !await categoryRepository.IsAnyExistAsync(n =>
                            n.Name == request.Name && n.Id != request.Id
                        )
                )
                .WithMessage(ValidationMessages.Category.CategoryExist);
        }
    }
}
