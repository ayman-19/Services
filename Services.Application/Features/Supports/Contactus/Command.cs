using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Supports.Contactus
{
    public sealed record ContactusCommand(
        string Name,
        string Phone,
        string Gmail,
        string? Address,
        string Content
    ) : IRequest<Response>;
}
