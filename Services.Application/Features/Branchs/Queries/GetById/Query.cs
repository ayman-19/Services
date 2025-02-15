using MediatR;
using Services.Application.Features.Services.Queries.GetById;
using Services.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Features.Branchs.Queries.GetById
{
	public sealed record GetBranchQuery(Guid Id) : IRequest<ResponseOf<GetBranchResult>>;
	
}
