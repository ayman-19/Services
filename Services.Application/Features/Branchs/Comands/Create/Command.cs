using MediatR;
using Services.Domain.Entities;
using Services.Shared.Responses;

namespace Services.Application.Features.Branchs.Comands.Create
{
    public sealed record CreateBranchCommand(string name, double langtuide, double latitude)
        : IRequest<ResponseOf<CreateBranchResult>>
    {
        public static implicit operator Branch(CreateBranchCommand branchCommand) =>
            new()
            {
                Name = branchCommand.name,
                Langitude = branchCommand.langtuide,
                Latitude = branchCommand.latitude,
            };
    }
}
