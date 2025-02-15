using System;
using MediatR;
using Services.Domain.Entities;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Commands.AssignWorkerToService
{
    public sealed record AssignWorkerToServiceCommand(Guid WorkerId, Guid ServiceId, Guid BranchId)
        : IRequest<ResponseOf<AssignWorkerToServiceResult>>
    {
        public static implicit operator WorkerService(AssignWorkerToServiceCommand command) =>
            new()
            {
                WorkerId = command.WorkerId,
                ServiceId = command.ServiceId,
                BranchId = command.BranchId,
            };
    }
}
