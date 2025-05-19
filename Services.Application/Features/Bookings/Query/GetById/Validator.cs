using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Bookings.Query.GetById
{
    public sealed class GetBookingsValidator : AbstractValidator<GetBookingByIdQuery>
    {
        private readonly IServiceProvider _serviceProvider;

        public GetBookingsValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IBookingRepository>());
        }

        private void ValidateRequest(IBookingRepository bookingRepository)
        {
            RuleFor(bo => bo.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Booking.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Booking.IdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await bookingRepository.IsAnyExistAsync(bo => bo.Id == id)
                )
                .WithMessage(ValidationMessages.Booking.BookingNotExist);
        }
    }
}
