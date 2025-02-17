namespace Services.Application.Features.Branchs.Queries.Paginate
{
    public sealed record PaginateBranchResult(
        Guid id,
        string name,
        double langitude,
        double latitude
    );
}
