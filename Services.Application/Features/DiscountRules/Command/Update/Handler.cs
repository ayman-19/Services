using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.DiscountRules.Command.Update
{
    public sealed record UpdateDiscountRulesHandler(
        IDiscountRuleRepository _discountrulesRepository,
        IUnitOfWork _unitOfWork
    ) : IRequestHandler<UpdateDiscountRulesCommand, ResponseOf<UpdateDiscountRulesResult>>
    {
        public async Task<ResponseOf<UpdateDiscountRulesResult>> Handle(
            UpdateDiscountRulesCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    DiscountRule discount = await _discountrulesRepository.GetByIdAsync(
                        request.Id,
                        cancellationToken
                    );
                    discount.UpdateDiscountRole(request.MainPoints, request.DiscountId);
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
