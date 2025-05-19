using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Application.Features.Branchs.Comands.Delete;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Bookings.Command.Delete
{
    public sealed class DeleteBookingValidator : AbstractValidator<DeleteBookingCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteBookingValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IBookingRepository>());
        }

        private void ValidateRequest(IBookingRepository bookRepository)
        {
            RuleFor(book => book.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Bookings.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Bookings.IdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await bookRepository.IsAnyExistAsync(book => book.Id == id)
                )
                .WithMessage(ValidationMessages.Bookings.BookingDoesNotExist);
        }
    }
}
