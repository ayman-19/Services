using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.DiscountRules.Command.Update
{
    public sealed record UpdateDiscountRulesCommand(Guid Id, Guid DiscountId, int MainPoints)
        : IRequest<ResponseOf<UpdateDiscountRulesResult>>;
}
