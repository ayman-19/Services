using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Commands.RemoveWorkerFromService
{
    public sealed record RemoveWorkerFromServiceCommand(Guid WorkerId, Guid ServiceId)
        : IRequest<Response>;
}
