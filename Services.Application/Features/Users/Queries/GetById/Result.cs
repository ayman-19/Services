namespace Services.Application.Features.Users.Queries.GetById
{
    public sealed record GetUserResult(
        Guid Id,
        string Name,
        string Email,
        DateTime CreatedOn,
        IEnumerable<string> roles
    );
}
