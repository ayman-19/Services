using MediatR;
using Services.Application.Features.Bookings.Command.Update;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;
using System.Net;


namespace Services.Application.Features.Discounts.Commands.Update
{
    public sealed record UpdateDiscountHandler : IRequestHandler<UpdateDiscountCommand, ResponseOf<UpdateDiscountResult>>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateDiscountHandler(IDiscountRepository discountRepository, IUnitOfWork unitOfWork)
        {
            _discountRepository = discountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseOf<UpdateDiscountResult>> Handle(
           UpdateDiscountCommand request,
           CancellationToken cancellationToken
       )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    Discount discount = await _discountRepository.GetByIdAsync(
                        request.Id,
                        cancellationToken
                    );
                    discount.UpdateDiscount(
                        request.Percentage
                    );
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
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new DatabaseTransactionException(ValidationMessages.Database.Error);
                }
            }



        }
    }
}