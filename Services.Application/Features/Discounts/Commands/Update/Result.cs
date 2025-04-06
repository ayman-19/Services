using Services.Domain.Entities;


namespace Services.Application.Features.Discounts.Commands.Update
{
	public sealed record UpdateDiscountResult
		(Guid Id ,
		double Percentage)
	{
		public static implicit operator UpdateDiscountResult(Discount d)=>
			new(d.Id , d.Percentage);	

    }
}
