using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Shared.Context;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Branchs.Comands.Update
{
    public sealed class UpdateBranchHandler
        : IRequestHandler<UpdateBranchCommand, ResponseOf<UpdateBranchResult>>
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;

        public UpdateBranchHandler(
            IBranchRepository branchRepository,
            IUnitOfWork unitOfWork,
            IUserContext userContext
        )
        {
            _branchRepository = branchRepository;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task<ResponseOf<UpdateBranchResult>> Handle(
            UpdateBranchCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var userContext = _userContext.UserId;
                    if (!userContext.Exist)
                        throw new Exception("Not Authorized.");

                    Branch branch = await _branchRepository.GetByIdAsync(
                        Guid.Parse(userContext.Value),
                        cancellationToken
                    );
                    branch.UpdaeBranch(request.langtuide, request.latitude);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return new()
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = branch,
                    };
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new DatabaseTransactionException(ValidationMessages.Database.Error);
                }
            }
        }
    }
}
