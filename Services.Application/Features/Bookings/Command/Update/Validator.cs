using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Domain.Enums;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Bookings.Command.Update
{
    public class UpdateBookingValidator : AbstractValidator<UpdateBookingCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdateBookingValidator(IServiceProvider serviceProvider)
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
            RuleFor(b => b.Location)
                .NotEmpty()
                .WithMessage(ValidationMessages.Bookings.LocationIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Bookings.LocationIsRequired);

            RuleFor(b => b.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Bookings.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Bookings.IdIsRequired);

            RuleFor(b => b.CustomerId)
                .NotEmpty()
                .WithMessage(ValidationMessages.Bookings.CustomerIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Bookings.CustomerIdIsRequired);

            RuleFor(b => b.WorkerId)
                .NotEmpty()
                .WithMessage(ValidationMessages.Bookings.WorkerIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Bookings.WorkerIdIsRequired);

            RuleFor(b => b)
                .Must(b =>
                    b.Status == BookingStatus.Completed
                        ? b.Rate > 0
                            ? true
                            : false
                        : true
                )
                .WithMessage(ValidationMessages.Services.RateIsRequired);

            RuleFor(b => b.ServiceId)
                .MustAsync(
                    async (id, cancellationToken) =>
                        await serviceRepository.IsAnyExistAsync(s => s.Id == id)
                )
                .WithMessage(ValidationMessages.Services.ServiceDoesNotExist);

            RuleFor(b => b.WorkerId)
                .MustAsync(
                    async (id, cancellationToken) =>
                        await workerRepository.IsAnyExistAsync(s => s.UserId == id)
                )
                .WithMessage(ValidationMessages.Workers.WorkerDoesNotExist);

            RuleFor(b => b.CustomerId)
                .MustAsync(
                    async (id, cancellationToken) =>
                        await customerRepository.IsAnyExistAsync(s => s.UserId == id)
                )
                .WithMessage(ValidationMessages.Customers.CustomerDoesNotExist);

            RuleFor(b => b.Id)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await bookingRepository.IsAnyExistAsync(b => b.Id == id)
                )
                .WithMessage(ValidationMessages.Bookings.BookingDoesNotExist);
        }
    }
}
