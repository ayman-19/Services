using Services.Domain.Entities;

namespace Services.Application.Features.Services.Commands.Create
{
    public sealed record CreateServiceResult(Guid id, string name, string description, string iamge)
    {
        public static implicit operator CreateServiceResult(Service service) =>
            new(service.Id, service.Name, service.Description, service.Image);
    }
}
