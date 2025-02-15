using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Features.Branchs.Queries.GetById
{

    public sealed record GetBranchResult(Guid id,string name ,double langtude,double latitude);
	
}
