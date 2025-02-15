using MediatR;
using Services.Application.Features.Services.Queries.GetById;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Features.Branchs.Queries.GetById
{
    public sealed class GetBranchHandler : IRequestHandler<GetBranchQuery, ResponseOf<GetBranchResult>>
    {

        private readonly IBranchRepository _branchRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GetBranchHandler(IBranchRepository branchRepository, IUnitOfWork unitOfWork)
        {
            _branchRepository = branchRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseOf<GetBranchResult>> Handle(
            GetBranchQuery request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var result = await _branchRepository.GetAsync(
                        b => b.Id == request.Id,
                        b => new GetBranchResult(b.Id, b.Name, b.Langitude,b.Latitude),
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
                catch
                {
                    throw new DatabaseTransactionException(ValidationMessages.Database.Error);
                }
            }
        }
    }
}
