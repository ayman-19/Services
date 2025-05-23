﻿using MediatR;
using Services.Domain.Enums;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetWorkersOnService
{
    public sealed record GetWorkersOnServiceQuery(
        Guid ServiceId,
        Guid? WorkerId,
        double Latitude,
        double Longitude,
        Status? Status
    ) : IRequest<ResponseOf<GetWorkersOnServiceResult>>;
}
