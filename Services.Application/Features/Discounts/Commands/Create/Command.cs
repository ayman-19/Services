using MediatR;
using Services.Shared.Responses;
using Services.Domain.Entities;

namespace Services.Application.Features.Discounts.Commands.Create
{
	public sealed record CreateDiscountCommand(
		double Percentage,
		DateTime Expireon
		) : IRequest<ResponseOf<CreateDiscountResult>>
	{
		public static implicit operator Discount(CreateDiscountCommand command) =>
			new()
			{
				Percentage = command.Percentage,
				ExpireOn = command.Expireon,	
			};
	}
	
}
