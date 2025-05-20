using Services.Domain.Entities;

namespace Services.Application.Features.DiscountRules.Command.Update
{
    public sealed record UpdateDiscountRulesResult(
        Guid DiscountRuleId,
        Guid DiscountId,
        int MainPoints
    )
    {
        public static implicit operator UpdateDiscountRulesResult(DiscountRule d) =>
            new(d.Id, d.DiscountId, d.MainPoints);
    }
}
