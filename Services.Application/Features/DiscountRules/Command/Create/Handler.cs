﻿using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.DiscountRules.Command.Create
{
    public sealed record CreateDiscountRulesHandler(
        IDiscountRuleRepository _discountruleRepository,
        IUnitOfWork _unitOfWork
    ) : IRequestHandler<CreateDiscountRulesCommand, ResponseOf<CreateDiscountRuleResult>>
    {
        public async Task<ResponseOf<CreateDiscountRuleResult>> Handle(
            CreateDiscountRulesCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    DiscountRule discountrule = request;
                    await _discountruleRepository.CreateAsync(discountrule, cancellationToken);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return new()
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = discountrule,
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
