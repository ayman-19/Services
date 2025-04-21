using System.Net;
using MediatR;
using Services.Application.Features.Services.Queries.Paginate;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Branchs.Queries.Paginate
{
    public sealed class PaginateBranchHandler
        : IRequestHandler<
            PaginateBranchQuery,
            ResponseOf<IReadOnlyCollection<PaginateBranchResult>>
        >
    {
        private readonly IBranchRepository _branchRepository;

        public PaginateBranchHandler(IBranchRepository branchRepository) =>
            _branchRepository = branchRepository;

        public async Task<ResponseOf<IReadOnlyCollection<PaginateBranchResult>>> Handle(
            PaginateBranchQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                IReadOnlyCollection<PaginateBranchResult>? result =
                    await _branchRepository.PaginateAsync(
                        request.page == 0 ? 1 : request.page,
                        request.pageSize == 0 ? 10 : request.pageSize,
                        b => new PaginateBranchResult(b.Id, b.Langitude, b.Latitude),
                        b => b.Id == request.Id || request.Id == null,
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
