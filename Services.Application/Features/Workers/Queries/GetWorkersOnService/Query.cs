﻿using MediatR;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetWorkersOnService
{
    public sealed record GetWorkersOnServiceQuery(Guid ServiceId, Guid? WorkerId)
        : IRequest<ResponseOf<GetWorkersOnServiceResult>>;
}
