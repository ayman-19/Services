using System.Net;
using MediatR;
using Services.Application.Features.Branchs.Queries.Paginate;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Branchs.Queries.GetAll
{
    public sealed class GetAllBranchsHandler
        : IRequestHandler<GetAllBranchsQuery, ResponseOf<IReadOnlyCollection<GetAllBranchsResult>>>
    {
        private readonly IBranchRepository _branchRepository;

        public GetAllBranchsHandler(IBranchRepository branchRepository) =>
            _branchRepository = branchRepository;

        public async Task<ResponseOf<IReadOnlyCollection<GetAllBranchsResult>>> Handle(
            GetAllBranchsQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                IReadOnlyCollection<GetAllBranchsResult>? result =
                    await _branchRepository.GetAllAsync(
                        b => new GetAllBranchsResult(b.Id, b.Name, b.Description),
                        null!,
                        null!,
                        cancellationToken
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
