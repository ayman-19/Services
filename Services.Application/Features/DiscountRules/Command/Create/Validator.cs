using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.DiscountRules.Command.Create
{
    public sealed class CreateDiscountRulesValidator : AbstractValidator<CreateDiscountRulesCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public CreateDiscountRulesValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(
                scope.ServiceProvider.GetRequiredService<IDiscountRuleRepository>(),
                scope.ServiceProvider.GetRequiredService<IDiscountRepository>()
            );
        }

        private void ValidateRequest(
            IDiscountRuleRepository discountRuleRepository,
            IDiscountRepository discountRepository
        )
        {
            RuleFor(D => D.DiscountId)
                .NotEmpty()
                .WithMessage(ValidationMessages.DiscountRule.DiscountIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.DiscountRule.DiscountIdIsRequired);

            RuleFor(D => D.MainPoints)
                .NotEmpty()
                .WithMessage(ValidationMessages.DiscountRule.MainPointIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.DiscountRule.MainPointIsRequired);

            RuleFor(D => D.MainPoints)
                .MustAsync(
                    async (point, cancellationToken) =>
                        !await discountRuleRepository.IsAnyExistAsync(d => d.MainPoints == point)
                )
                .WithMessage(ValidationMessages.DiscountRule.MainPointIsExist);

            RuleFor(D => D.DiscountId)
                .MustAsync(
                    async (id, cancellationToken) =>
                        await discountRepository.IsAnyExistAsync(d => d.Id == id)
                )
                .WithMessage(ValidationMessages.Discounts.DiscountDoesNotExist);
        }
    }
}
