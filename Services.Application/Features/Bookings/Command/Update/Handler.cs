using System.Net;
using MediatR;
using Microsoft.Extensions.Logging;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Domain.Enums;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Bookings.Command.Update
{
    public sealed record UpdateBookingHandler(
        IBookingRepository BookingRepository,
        IUnitOfWork UnitOfWork,
        IJobs Jobs,
        IDiscountRuleRepository DiscountRuleRepository
    ) : IRequestHandler<UpdateBookingCommand, ResponseOf<UpdateBookingResult>>
    {
        public async Task<ResponseOf<UpdateBookingResult>> Handle(
            UpdateBookingCommand request,
            CancellationToken cancellationToken
        )
        {
            using var transaction = await UnitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var booking = await BookingRepository.GetByIdAsync(request.Id, cancellationToken);

                var (discountPoints, discountPercentage) =
                    await DiscountRuleRepository.GetPercentageOfPoint(
                        booking.Customer.Point.Number,
                        cancellationToken
                    );

                var discountedPrice = CalculateDiscountedPrice(
                    request.Total,
                    discountPoints,
                    discountPercentage
                );

                booking.UpdateBooking(
                    request.CreateOn,
                    request.Location,
                    request.CustomerId,
                    request.WorkerId,
                    request.ServiceId,
                    request.IsPaid,
                    request.Total,
                    discountedPrice,
                    request.Rate
                );

                await UnitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                if (booking.Status == BookingStatus.Completed && booking.IsPaid)
                {
                    await Jobs.RateWorkersAsync(
                        booking.WorkerId,
                        booking.ServiceId,
                        booking.CustomerId
                    );
                }
                return new()
                {
                    Message = ValidationMessages.Success,
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = booking,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new Exception(ex.Message, ex);
            }
        }

        private static double CalculateDiscountedPrice(
            double totalPrice,
            int discountPoints,
            double discountPercentage
        )
        {
            if (discountPoints == 0 || discountPercentage <= 0)
                return totalPrice;

            var discountAmount = totalPrice * (discountPercentage / 100);
            return Math.Max(totalPrice - discountAmount, 0);
        }
    }
}
