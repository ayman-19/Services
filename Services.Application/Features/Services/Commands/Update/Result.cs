using Services.Domain.Entities;

namespace Services.Application.Features.Services.Commands.Update
{
    public sealed record UpdateServiceResult(Guid id, string name, string description)
    {
        public static implicit operator UpdateServiceResult(Service service) =>
            new(service.Id, service.Name, service.Description);
    }
}
