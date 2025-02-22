using Services.Domain.Entities;

namespace Services.Application.Features.Branchs.Comands.Create
{
    public sealed record CreateBranchResult(
        Guid Id,
        string name,
        string description,
        double langtuide,
        double latitude
    )
    {
        public static implicit operator CreateBranchResult(Branch branch) =>
            new(branch.Id, branch.Name, branch.Description, branch.Latitude, branch.Langitude);
    }
}
