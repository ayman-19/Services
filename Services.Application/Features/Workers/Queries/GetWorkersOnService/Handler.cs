using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetWorkersOnService
{
    //public sealed record GetWorkersOnServiceHandler
    //    : IRequestHandler<GetWorkersOnServiceQuery, ResponseOf<GetWorkersOnServiceResult>>
    //{
    //    private readonly IWorkerServiceRepository _workerServiceRepository;
    //
    //    public GetWorkersOnServiceHandler(IWorkerServiceRepository workerServiceRepository) =>
    //        _workerServiceRepository = workerServiceRepository;
    //
    //    public async Task<ResponseOf<GetWorkersOnServiceResult>> Handle(
    //        GetWorkersOnServiceQuery request,
    //        CancellationToken cancellationToken
    //    )
    //    {
    //        try {var workersService  =  await _workerServiceRepository.GetAsync(ws=> ws.ServiceId /== /request.ServiceId, ws=> new GetWorkersOnServiceResult() }
    //        catch { }
    //    }
    //}
}
