using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Discounts.Commands.Delete
{
    public sealed class DeleteDiscountValidator : AbstractValidator<DeleteDiscountCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteDiscountValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IDiscountRepository>());
        }

        private void ValidateRequest(IDiscountRepository discountRepository)
        {
            RuleFor(discount => discount.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Discounts.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Discounts.IdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await discountRepository.IsAnyExistAsync(discount => discount.Id == id)
                )
                .WithMessage(ValidationMessages.Discounts.DiscountExists);
        }
    }
}
