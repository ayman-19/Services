using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Services.Application.Features.Workers.Queries.GetAll
{
    public sealed record GetAllWorkerQuery() : IRequest { }
}
