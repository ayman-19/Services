using Services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Features.Branchs.Comands.Create
{
   
    public sealed  record CreateBranchResult(Guid Id,string name ,double langtuide ,double latitude)
	{
     public static implicit operator CreateBranchResult(Branch branch) => new(branch.Id,branch.Name,branch.Latitude,branch.Langitude);

    }
}
