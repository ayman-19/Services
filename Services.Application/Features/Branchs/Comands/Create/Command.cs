using MediatR;
using Services.Application.Features.Services.Commands.Create;
using Services.Domain.Entities;
using Services.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Features.Branchs.Comands.Create
{
    
    public sealed record CreateBranchCommand(string name,double langtuide,double latitude):IRequest<ResponseOf<CreateBranchResult>>
	{
        public static implicit operator Branch(CreateBranchCommand branchCommand) =>
            new() { Name = branchCommand.name, Langitude= branchCommand.langtuide,Latitude = branchCommand.latitude };
    }
}
