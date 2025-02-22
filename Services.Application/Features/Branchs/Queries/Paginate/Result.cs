namespace Services.Application.Features.Branchs.Queries.Paginate
{
    public sealed record PaginateBranchResult(
        Guid id,
        string name,
        string description,
        double langitude,
        double latitude
    );
}
