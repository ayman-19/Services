using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Commands.AssignWorkerToService
{
    public sealed class AssignWorkerToServiceValidator
        : AbstractValidator<AssignWorkerToServiceCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public AssignWorkerToServiceValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(
                scope.ServiceProvider.GetRequiredService<IWorkerRepository>(),
                scope.ServiceProvider.GetRequiredService<IWorkerServiceRepository>(),
                scope.ServiceProvider.GetRequiredService<IServiceRepository>(),
                scope.ServiceProvider.GetRequiredService<IBranchRepository>()
            );
        }

        private void ValidateRequest(
            IWorkerRepository workerRepository,
            IWorkerServiceRepository workerServiceRepository,
            IServiceRepository serviceRepository,
            IBranchRepository branchRepository
        )
        {
            RuleFor(s => s.WorkerId)
                .NotEmpty()
                .WithMessage(ValidationMessages.WorkerServices.WorkerIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.WorkerServices.WorkerIdIsRequired);

            RuleFor(s => s.ServiceId)
                .NotEmpty()
                .WithMessage(ValidationMessages.WorkerServices.ServiceIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.WorkerServices.ServiceIdIsRequired);

            RuleFor(w => w.WorkerId)
                .MustAsync(
                    async (workerId, CancellationToken) =>
                        await workerRepository.IsAnyExistAsync(n => n.UserId == workerId)
                )
                .WithMessage(ValidationMessages.WorkerServices.WorkerDoesNotExist);

            RuleFor(s => s.ServiceId)
                .MustAsync(
                    async (serviceId, CancellationToken) =>
                        await serviceRepository.IsAnyExistAsync(n => n.Id == serviceId)
                )
                .WithMessage(ValidationMessages.WorkerServices.ServiceDoesNotExist);

            RuleFor(command => command)
                .MustAsync(
                    async (command, CancellationToken) =>
                        !await workerServiceRepository.IsAnyExistAsync(n =>
                            n.WorkerId == command.WorkerId && n.ServiceId == command.ServiceId
                        )
                )
                .WithMessage(ValidationMessages.WorkerServices.WorkerAlreadyAssignedToService);
        }
    }
}
