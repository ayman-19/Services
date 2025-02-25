using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Services.Application.Features.Workers.Queries.GetAllServicesWithWorkers;
using Services.Shared.Responses;

namespace Services.Application.Features.Workers.Queries.GetAll
{
    public class GetAllWorkerQuery : IRequest<ResponseOf<IReadOnlyCollection<GetAllWorkerResult>>>;
}
