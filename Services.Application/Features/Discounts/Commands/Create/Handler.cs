using MediatR;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;
using System.Net;


namespace Services.Application.Features.Discounts.Commands.Create
{
    public sealed record CreateDiscountHandler
        : IRequestHandler<CreateDiscountCommand, ResponseOf<CreateDiscountResult>>
    {

        private readonly IDiscountRepository _discountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateDiscountHandler(IDiscountRepository discountRepository, IUnitOfWork unitOfWork)
        {
            _discountRepository = discountRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseOf<CreateDiscountResult>>
            Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    Discount discount = request;
                    await _discountRepository.CreateAsync(discount, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return new()
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = discount,
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new Exception(ex.Message, ex);
                }
            }
        }
    }
}
