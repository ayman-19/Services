using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.DiscountRules.Command.Delete
{
    public sealed record DeleteDiscountRulesCommand(Guid Id) : IRequest<Response>;
}
