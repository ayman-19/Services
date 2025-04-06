namespace Services.Application.Features.Discounts.Queries.GetById
{
    public sealed record GetDiscountByIdResult(Guid Id, double Percentage, DateTime ExpireOn);
}
