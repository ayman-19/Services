using MediatR;
using Services.Domain.Enums;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Commands.UpdateWorkerStatus
{
    public sealed record UpdateWorkerStatusCommand(Guid WorkerId, Status Status)
        : IRequest<Response>;
}
