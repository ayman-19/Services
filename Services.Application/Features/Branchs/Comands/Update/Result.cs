using Services.Domain.Entities;

namespace Services.Application.Features.Branchs.Comands.Update
{
    public sealed record UpdateBranchResult(Guid id ,string branchName,double langtuide ,double latitude)
    {
        public static implicit operator UpdateBranchResult(Branch b) =>
             new(b.Id,b.Name,b.Langitude,b.Latitude);
    }
}
