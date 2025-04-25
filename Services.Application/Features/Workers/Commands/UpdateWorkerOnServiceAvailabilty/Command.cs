using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Commands.UpdateWorkerOnServiceAvailabilty
{
    public sealed record UpdateWorkerOnServiceAvailabiltyCommand(
        Guid WorkerId,
        Guid ServiceId,
        bool Availabilty
    ) : IRequest<Response>;
}
