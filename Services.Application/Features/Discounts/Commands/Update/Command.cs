using MediatR;
using Services.Domain.Entities;
using Services.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Features.Discounts.Commands.Update
{
    public sealed record UpdateDiscountCommand(
        Guid Id,
        double Percentage
   ):IRequest<ResponseOf<UpdateDiscountResult>>;
}
