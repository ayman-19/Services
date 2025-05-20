using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.DiscountRules.Command.Delete
{
    public sealed class DeleteDiscountRulesValidator : AbstractValidator<DeleteDiscountRulesCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteDiscountRulesValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IDiscountRuleRepository>());
        }

        private void ValidateRequest(IDiscountRuleRepository discountruleRepository)
        {
            RuleFor(d => d.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.DiscountRule.DiscountIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.DiscountRule.DiscountIdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await discountruleRepository.IsAnyExistAsync(d => d.Id == id)
                )
                .WithMessage(ValidationMessages.DiscountRule.IdIsNotIsExist);
        }
    }
}
