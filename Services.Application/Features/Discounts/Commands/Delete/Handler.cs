using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;
using System.Net;
namespace Services.Application.Features.Discounts.Commands.Delete
{
  
    public sealed record DeleteDiscountHandler : IRequestHandler<DeleteDiscountCommand, Response>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDiscountHandler(IDiscountRepository discountRepository, IUnitOfWork unitOfWork)
        {
            _discountRepository = discountRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response> Handle(
              DeleteDiscountCommand request,
              CancellationToken cancellationToken
          )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    await _discountRepository.DeleteByIdAsync(request.Id, cancellationToken);
                    await transaction.CommitAsync();
                    return new()
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                    };
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw new DatabaseTransactionException(ValidationMessages.Database.Error);
                }
            }
        }
    }
}
