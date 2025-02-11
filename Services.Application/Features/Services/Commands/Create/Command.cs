using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Services.Commands.Create
{
     public sealed record CreateServiceCommand(string name, string description) : IRequest<ResponseOf<CreateServiceResult>>;
}
