using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Services.Commands.Delete
{
    public sealed record DeleteServiceCommand(Guid Id) : IRequest<Response>;
}
