using Services.Domain.Entities;

namespace Services.Application.Features.DiscountRules.Command.Create
{
	public sealed record CreateDiscountRuleResult(
		Guid Id,
        Guid DiscountId,
		int MainPoints
        )
	{
		public static implicit operator CreateDiscountRuleResult(DiscountRule discountRule) =>
			new(discountRule.Id,
				discountRule.DiscountId,
				discountRule.MainPoints)
			;
			
			
    }
}
