using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Services.Commands.Create
{
    public sealed class CreateServiceHandler : IRequestHandler<CreateServiceCommand, ResponseOf<CreateServiceResult>>
    {
        public Task<ResponseOf<CreateServiceResult>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
