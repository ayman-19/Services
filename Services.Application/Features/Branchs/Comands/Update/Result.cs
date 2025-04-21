using Services.Domain.Entities;

namespace Services.Application.Features.Branchs.Comands.Update
{
    public sealed record UpdateBranchResult(Guid id, double langtuide, double latitude)
    {
        public static implicit operator UpdateBranchResult(Branch b) =>
            new(b.Id, b.Langitude, b.Latitude);
    }
}
