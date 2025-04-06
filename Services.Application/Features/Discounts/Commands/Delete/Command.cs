using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Discounts.Commands.Delete
{
	public sealed record DeleteDiscountCommand(Guid Id):IRequest<Response>;
		
}
