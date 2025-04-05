using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Bookings.Command.Create
{
    public sealed record CreateBookingHandler
        : IRequestHandler<CreateBookingCommand, ResponseOf<CreateBookingResult>>
    {
        private readonly IBookingRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookingHandler(IBookingRepository bookRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseOf<CreateBookingResult>> Handle(
            CreateBookingCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    Booking book = request;
                    await _bookRepository.CreateAsync(book, cancellationToken);
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
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new Exception(ex.Message, ex);
                }
            }
        }
    }
}
