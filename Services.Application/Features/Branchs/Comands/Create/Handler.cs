using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Branchs.Comands.Create
{
    public sealed record CreateBranchHandler
        : IRequestHandler<CreateBranchCommand, ResponseOf<CreateBranchResult>>
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBranchHandler(IBranchRepository branchRepository, IUnitOfWork unitOfWork)
        {
            _branchRepository = branchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseOf<CreateBranchResult>> Handle(
            CreateBranchCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    Branch branch = request;
                    await _branchRepository.CreateAsync(branch, cancellationToken);
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
