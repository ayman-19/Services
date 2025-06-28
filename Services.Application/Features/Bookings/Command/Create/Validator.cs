using FluentValidation;
using Services.Domain.Abstraction;
using Services.Domain.Enums;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Bookings.Command.Create;

public sealed class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IWorkerRepository _workerRepository;
    private readonly IServiceRepository _serviceRepository;

    public CreateBookingValidator(
        IBookingRepository bookingRepository,
        ICustomerRepository customerRepository,
        IWorkerRepository workerRepository,
        IServiceRepository serviceRepository
    )
    {
        _bookingRepository = bookingRepository;
        _customerRepository = customerRepository;
        _workerRepository = workerRepository;
        _serviceRepository = serviceRepository;

        RuleLevelCascadeMode = CascadeMode.Stop;
        ClassLevelCascadeMode = CascadeMode.Stop;

        ApplyValidationRules();
    }

    private void ApplyValidationRules()
    {
        RuleFor(x => x.LocationType)
            .IsInEnum()
            .WithMessage(ValidationMessages.Bookings.LocationIsRequired);

        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage(ValidationMessages.Bookings.CustomerIdIsRequired)
            .MustAsync(CustomerExists)
            .WithMessage(ValidationMessages.Customers.CustomerDoesNotExist)
            .MustAsync(NoUnpaidCompletedBooking)
            .WithMessage(ValidationMessages.Bookings.UnpaidPreviousBooking)
            .MustAsync(NoUnratedCompletedBooking)
            .WithMessage(ValidationMessages.Bookings.RateNotProvided);

        RuleFor(x => x.WorkerId)
            .NotEmpty()
            .WithMessage(ValidationMessages.Bookings.WorkerIdIsRequired)
            .MustAsync(WorkerExists)
            .WithMessage(ValidationMessages.Workers.WorkerDoesNotExist);

        RuleFor(x => x.ServiceId)
            .MustAsync(ServiceExists)
            .WithMessage(ValidationMessages.Services.ServiceDoesNotExist);

        RuleFor(x => x)
            .MustAsync(NoPendingDuplicateBooking)
            .WithMessage(ValidationMessages.Bookings.NoBooking);
    }

    private async Task<bool> ServiceExists(Guid serviceId, CancellationToken ct) =>
        await _serviceRepository.IsAnyExistAsync(s => s.Id == serviceId);

    private async Task<bool> WorkerExists(Guid workerId, CancellationToken ct) =>
        await _workerRepository.IsAnyExistAsync(w => w.UserId == workerId);

    private async Task<bool> CustomerExists(Guid customerId, CancellationToken ct) =>
        await _customerRepository.IsAnyExistAsync(c => c.UserId == customerId);

    private async Task<bool> NoPendingDuplicateBooking(
        CreateBookingCommand cmd,
        CancellationToken ct
    ) =>
        !await _bookingRepository.IsAnyExistAsync(b =>
            b.CustomerId == cmd.CustomerId
            && b.WorkerId == cmd.WorkerId
            && b.Status == BookingStatus.Pending
        );

    private async Task<bool> NoUnpaidCompletedBooking(Guid customerId, CancellationToken ct) =>
        !await _bookingRepository.IsAnyExistAsync(b =>
            b.CustomerId == customerId && b.Status == BookingStatus.Completed && b.IsPaid == false
        );

    private async Task<bool> NoUnratedCompletedBooking(Guid customerId, CancellationToken ct) =>
        !await _bookingRepository.IsAnyExistAsync(b =>
            b.CustomerId == customerId && b.Status == BookingStatus.Completed && b.Rate == 0
        );
}
