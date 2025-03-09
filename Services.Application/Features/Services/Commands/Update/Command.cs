using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Services.Commands.Update
{
    public sealed record UpdateServiceCommand(
        Guid id,
        string name,
        string description,
        Guid categoryId
    ) : IRequest<ResponseOf<UpdateServiceResult>>;
}
