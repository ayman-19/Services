using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.DiscountRules.Command.Delete
{
    public sealed record DeleteDiscountRuleHandler(IDiscountRuleRepository _discountrulesRepository)
        : IRequestHandler<DeleteDiscountRulesCommand, Response>
    {
        public async Task<Response> Handle(
            DeleteDiscountRulesCommand request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                await _discountrulesRepository.DeleteByIdAsync(request.Id, cancellationToken);
                return new()
                {
                    Message = ValidationMessages.Success,
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }
            catch
            {
                throw new DatabaseTransactionException(ValidationMessages.Database.Error);
            }
        }
    }
}
