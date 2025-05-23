using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.DiscountRules.Command.Update
{
    public sealed class UpdateDiscountRuleValidator : AbstractValidator<UpdateDiscountRulesCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdateDiscountRuleValidator(IServiceProvider serviceProvider)
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
            IDiscountRuleRepository discountruleRepository,
            IDiscountRepository discountRepository
        )
        {
            RuleFor(d => d.DiscountId)
                .NotEmpty()
                .WithMessage(ValidationMessages.DiscountRule.DiscountIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.DiscountRule.DiscountIdIsRequired);

            RuleFor(d => d.MainPoints)
                .NotEmpty()
                .WithMessage(ValidationMessages.DiscountRule.MainPointIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.DiscountRule.MainPointIsRequired);

            RuleFor(D => D)
                .MustAsync(
                    async (mainpoint, cancellationToken) =>
                        !await discountruleRepository.IsAnyExistAsync(d =>
                            d.MainPoints == mainpoint.MainPoints && d.Id != mainpoint.Id
                        )
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
