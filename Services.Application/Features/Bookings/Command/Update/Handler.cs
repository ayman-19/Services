using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Domain.Enums;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Bookings.Command.Update
{
    public sealed record UpdateBookingHandler
        : IRequestHandler<UpdateBookingCommand, ResponseOf<UpdateBookingResult>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJobs _jobs;

        public UpdateBookingHandler(
            IBookingRepository bookingRepository,
            IUnitOfWork unitOfWork,
            IJobs jobs
        )
        {
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
            _jobs = jobs;
        }

        public async Task<ResponseOf<UpdateBookingResult>> Handle(
            UpdateBookingCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    Booking book = await _bookingRepository.GetByIdAsync(
                        request.Id,
                        cancellationToken
                    );
                    book.UpdateBooking(
                        request.CreateOn,
                        request.Location,
                        request.CustomerId,
                        request.WorkerId,
                        request.ServiceId,
                        request.IsPaid,
                        request.Total,
                        request.Rate
                    );
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    if (book.Status == BookingStatus.Completed && book.IsPaid)
                        await _jobs.RateWorkersAsync(
                            book.WorkerId,
                            book.ServiceId,
                            book.CustomerId
                        );

                    return new()
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = book,
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new Exception(ex.Message, ex);
                }
            }
        }
    }
}
