using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Branchs.Comands.Update
{
    public sealed record UpdateBranchCommand(Guid Id, double langtuide, double latitude)
        : IRequest<ResponseOf<UpdateBranchResult>>;
}
