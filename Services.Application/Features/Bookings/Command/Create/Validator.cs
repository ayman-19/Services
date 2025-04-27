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
            ValidateRequest(
                scope.ServiceProvider.GetRequiredService<IBookingRepository>(),
                scope.ServiceProvider.GetRequiredService<ICustomerRepository>(),
                scope.ServiceProvider.GetRequiredService<IWorkerRepository>(),
                scope.ServiceProvider.GetRequiredService<IServiceRepository>()
            );
        }

        private void ValidateRequest(
            IBookingRepository bookingRepository,
            ICustomerRepository customerRepository,
            IWorkerRepository workerRepository,
            IServiceRepository serviceRepository
        )
        {
            RuleFor(s => s.LocationType)
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

            RuleFor(b => b.ServiceId)
                .MustAsync(
                    async (id, cancellationToken) =>
                        await serviceRepository.IsAnyExistAsync(s => s.Id == id)
                )
                .WithMessage(ValidationMessages.Service.ServiceNotExist);

            RuleFor(b => b.WorkerId)
                .MustAsync(
                    async (id, cancellationToken) =>
                        await workerRepository.IsAnyExistAsync(s => s.UserId == id)
                )
                .WithMessage(ValidationMessages.Workers.WorkereNotExist);

            RuleFor(b => b.CustomerId)
                .MustAsync(
                    async (id, cancellationToken) =>
                        await customerRepository.IsAnyExistAsync(s => s.UserId == id)
                )
                .WithMessage(ValidationMessages.Customers.CustomerNotExist);
        }
    }
}
