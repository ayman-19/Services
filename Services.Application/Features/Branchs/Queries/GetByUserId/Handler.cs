using System.Net;
using MediatR;
using Services.Application.Features.Branchs.Queries.GetById;
using Services.Domain.Abstraction;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Branchs.Queries.GetByUserId
{
    public sealed class GetBranchByUserIdHandler(
        IBranchRepository _branchRepository,
        IUnitOfWork _unitOfWork
    ) : IRequestHandler<GetBranchByUserIdQuery, ResponseOf<GetBranchResult>>
    {
        public async Task<ResponseOf<GetBranchResult>> Handle(
            GetBranchByUserIdQuery request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var result = await _branchRepository.GetAsync(
                        b => b.UserId == request.UserId,
                        b => new GetBranchResult(b.Id, b.Langitude, b.Latitude),
                        null!,
                        false,
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
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }
    }
}
