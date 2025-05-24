using Services.Domain.Enums;

namespace Services.Application.Features.Users.Queries.GetById
{
    public sealed record GetWorkerUserResult(
        Guid Id,
        string Name,
        string Email,
        string Phone,
        DateTime CreatedOn,
        IEnumerable<string> Roles,
        double Experience,
        Status Status
    );

    public sealed record GetCustomerUserResult(
        Guid Id,
        string Name,
        string Email,
        string Phone,
        DateTime CreatedOn,
        IEnumerable<string> Roles,
        int Points
    );
}
