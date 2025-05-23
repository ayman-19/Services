using MediatR;
using Services.Domain.Entities;
using Services.Shared.Responses;

namespace Services.Application.Features.DiscountRules.Command.Create
{
    public sealed record CreateDiscountRulesCommand(int MainPoints, Guid DiscountId)
        : IRequest<ResponseOf<CreateDiscountRuleResult>>
    {
        public static implicit operator DiscountRule(CreateDiscountRulesCommand command) =>
            new() { MainPoints = command.MainPoints, DiscountId = command.DiscountId };
    }
}
