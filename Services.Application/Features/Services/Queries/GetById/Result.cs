namespace Services.Application.Features.Services.Queries.GetById
{
    public sealed record GetServiceResult(Guid id, string name, string description);
}
