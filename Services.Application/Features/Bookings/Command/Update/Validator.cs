using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IBookingRepository>());
        }

        private void ValidateRequest(IBookingRepository bookingRepository)
        {
            RuleFor(b => b.Location)
                .NotEmpty()
                .WithMessage(ValidationMessages.Booking.LocationIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Booking.LocationIsRequired);

            RuleFor(b=>b.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Booking.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Booking.IdIsRequired);

            RuleFor(b => b.CustomerId)
                .NotEmpty()
                .WithMessage(ValidationMessages.Booking.CustomerIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Booking.CustomerIdIsRequired);

            RuleFor(b=>b.WorkerId)
                .NotEmpty()
                .WithMessage(ValidationMessages.Booking.WorkerIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Booking.WorkerIdIsRequired);


            RuleFor(b => b)
                .MustAsync(
                    async (request, CancellationToken) =>
                        !await bookingRepository.IsAnyExistAsync(n =>
                            n.CustomerId != request.CustomerId && n.WorkerId != request.WorkerId
                        )
                )
                .WithMessage(ValidationMessages.Booking.UserNotFound);
        }

    }
}
