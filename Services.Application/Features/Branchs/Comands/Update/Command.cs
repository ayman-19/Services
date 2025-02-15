using MediatR;
using Services.Application.Features.Services.Commands.Update;
using Services.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services.Application.Features.Branchs.Comands.Update
{
	public sealed record UpdateBranchCommand(Guid id ,string name ,double langtuide ,double latitude): IRequest<ResponseOf<UpdateBranchResult>>;
	
}
