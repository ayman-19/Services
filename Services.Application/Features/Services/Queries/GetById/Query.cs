using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Services.Queries.GetById
{
    public sealed record GetServiceQuery(Guid Id) : IRequest<ResponseOf<GetServiceResult>>;
}
