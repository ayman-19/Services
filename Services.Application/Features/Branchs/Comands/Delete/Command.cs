using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Branchs.Comands.Delete
{
    public sealed record DeleteBranchCommand(Guid Id) : IRequest<Response>;
}
