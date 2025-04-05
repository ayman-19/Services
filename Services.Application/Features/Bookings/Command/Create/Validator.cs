using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Bookings.Command.Create
{
    public sealed class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public CreateBookingValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IBookingRepository>());
        }

        private void ValidateRequest(IBookingRepository bookingRepository)
        {
            RuleFor(s => s.Location)
                .NotEmpty()
                .WithMessage(ValidationMessages.Booking.LocationIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Booking.LocationIsRequired);

            RuleFor(s => s.CustomerId)
                .NotEmpty()
                .WithMessage(ValidationMessages.Booking.CustomerIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Booking.CustomerIdIsRequired);

            RuleFor(s => s.WorkerId)
                .NotEmpty()
                .WithMessage(ValidationMessages.Booking.WorkerIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Booking.WorkerIdIsRequired);
        }
    }
}
