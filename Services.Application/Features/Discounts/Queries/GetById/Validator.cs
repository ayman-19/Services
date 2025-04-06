using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Discounts.Queries.GetById
{
    public sealed class GetDiscountByIdValidator : AbstractValidator<GetDiscountByIdQuery>
    {
        private readonly IServiceProvider _serviceProvider;

        public GetDiscountByIdValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IDiscountRepository>());
        }

        private void ValidateRequest(IDiscountRepository discountRepository)
        {
            RuleFor(dis => dis.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Discount.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Discount.IdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await discountRepository.IsAnyExistAsync(dis => dis.Id == id)
                )
                .WithMessage(ValidationMessages.Discount.DiscountNotExist);
        }
    }
}
