using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Bookings.Command.Update
{
    public sealed record UpdateBookingHandler
        : IRequestHandler<UpdateBookingCommand, ResponseOf<UpdateBookingResult>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBookingHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
        {
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
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
                        request.Total,
                        request.Rate
                    );
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return new()
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = book,
                    };
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new DatabaseTransactionException(ValidationMessages.Database.Error);
                }
            }
        }
    }
}
