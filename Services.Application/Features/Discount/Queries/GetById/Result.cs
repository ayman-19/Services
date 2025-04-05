namespace Services.Application.Features.Discount.Queries.GetById
{
    public sealed record GetDiscountByIdResult(Guid Id, double Percentage, DateTime ExpireOn);
}
