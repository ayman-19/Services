using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
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
                .WithMessage(ValidationMessages.Booking.LocationIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Booking.LocationIsRequired);

            RuleFor(b => b.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Booking.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Booking.IdIsRequired);

            RuleFor(b => b.CustomerId)
                .NotEmpty()
                .WithMessage(ValidationMessages.Booking.CustomerIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Booking.CustomerIdIsRequired);

            RuleFor(b => b.WorkerId)
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

            RuleFor(b => b.Id)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await bookingRepository.IsAnyExistAsync(b => b.Id == id)
                )
                .WithMessage(ValidationMessages.Booking.BookingNotExist);
        }
    }
}
