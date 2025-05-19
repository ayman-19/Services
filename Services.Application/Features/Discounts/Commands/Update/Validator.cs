using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Discounts.Commands.Update
{
    public sealed class UpdateDiscountValidator : AbstractValidator<UpdateDiscountCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdateDiscountValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IDiscountRepository>());
        }

        private void ValidateRequest(IDiscountRepository discountRepository)
        {
            RuleFor(d => d.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Discounts.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Discounts.IdIsRequired);

            RuleFor(d => d.Percentage)
                .NotEmpty()
                .WithMessage(ValidationMessages.Discounts.PercentageIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Discounts.PercentageIsRequired);

            RuleFor(b => b)
                .MustAsync(
                    async (request, CancellationToken) =>
                        !await discountRepository.IsAnyExistAsync(d =>
                            d.Percentage == request.Percentage && d.Id != request.Id
                        )
                )
                .WithMessage(ValidationMessages.Discounts.DiscountExists);
        }
    }
}
