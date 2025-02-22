namespace Services.Application.Features.Branchs.Queries.GetById
{
    public sealed record GetBranchResult(
        Guid id,
        string name,
        string description,
        double langtude,
        double latitude
    );
}
