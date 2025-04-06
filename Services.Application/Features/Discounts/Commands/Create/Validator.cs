using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Discounts.Commands.Create
{
	public sealed class CreateDiscountValidator:AbstractValidator<CreateDiscountCommand>
	{
        private readonly IServiceProvider _serviceProvider;

        public CreateDiscountValidator(IServiceProvider serviceProvider)
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
            RuleFor(d=>d.Percentage)
                .NotEmpty()
                .WithMessage(ValidationMessages.Discount.PercentageIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Discount.PercentageCantBeNull);

            RuleFor(d => d.Expireon)
                 .NotEmpty()
                 .WithMessage(ValidationMessages.Discount.ExpireDateIsRequired)
                 .NotNull()
                 .WithMessage(ValidationMessages.Discount.ExpireDateCantBeNull);

            RuleFor(d => d)
               .MustAsync(
                   async (request, CancellationToken) =>
                       !await discountRepository.IsAnyExistAsync(n =>
                           n.Percentage == request.Percentage 
                       )
               )
               .WithMessage(ValidationMessages.Discount.DiscountIsExist);

        }
    }
}
