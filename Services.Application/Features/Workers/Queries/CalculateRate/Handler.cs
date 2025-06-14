using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Queries.CalculateRate
{
    public sealed class CalculateRateHandler(IWorkerServiceRepository WorkerServiceRepository)
        : IRequestHandler<CalculateRateQuery, ResponseOf<CalculateRateResult>>
    {
        public async Task<ResponseOf<CalculateRateResult>> Handle(
            CalculateRateQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var result = await WorkerServiceRepository.GetRateAsync(
                    request.WorkerId,
                    cancellationToken
                );
                return new()
                {
                    Message = ValidationMessages.Success,
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = new(request.WorkerId, result),
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
