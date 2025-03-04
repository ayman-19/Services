using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Services.Application.Features.Branchs.Queries.Paginate;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Queries.GetWorkersBasedOnStatus
{
    public sealed record GetWorkerHandlerPaginate
        : IRequestHandler<
            GetWorkerQueryPaginate,
            ResponseOf<IReadOnlyCollection<GetWorkerResultPaginate>>
        >
    {
        private readonly IWorkerRepository _workerRepository;

        public GetWorkerHandlerPaginate(IWorkerRepository workerRepository) =>
            _workerRepository = workerRepository;

        public async Task<ResponseOf<IReadOnlyCollection<GetWorkerResultPaginate>>> Handle(
            GetWorkerQueryPaginate request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                IReadOnlyCollection<GetWorkerResultPaginate>? result =
                    await _workerRepository.PaginateAsync(
                        request.page == 0 ? 1 : request.page,
                        request.pagesize == 0 ? 10 : request.pagesize,
                        w => new GetWorkerResultPaginate(w.UserId, w.User.Name, w.Status),
                        null!
                    );

                return new()
                {
                    Message = ValidationMessages.Success,
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = result,
                };
            }
            catch
            {
                throw new DatabaseTransactionException(ValidationMessages.Database.Error);
            }
        }
    }
}
