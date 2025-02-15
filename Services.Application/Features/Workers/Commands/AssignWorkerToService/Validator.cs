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
                .WithMessage(ValidationMessages.WorkereService.WorkerIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.WorkereService.WorkerIdIsRequired);

            RuleFor(s => s.ServiceId)
                .NotEmpty()
                .WithMessage(ValidationMessages.WorkereService.ServiceIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.WorkereService.ServiceIdIsRequired);

            RuleFor(s => s.BranchId)
                .NotEmpty()
                .WithMessage(ValidationMessages.WorkereService.BranchIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.WorkereService.BranchIdIsRequired);

            RuleFor(w => w.WorkerId)
                .MustAsync(
                    async (workerId, CancellationToken) =>
                        await workerRepository.IsAnyExistAsync(n => n.UserId == workerId)
                )
                .WithMessage(ValidationMessages.WorkereService.WorkerNotExist);

            RuleFor(s => s.ServiceId)
                .MustAsync(
                    async (serviceId, CancellationToken) =>
                        await serviceRepository.IsAnyExistAsync(n => n.Id == serviceId)
                )
                .WithMessage(ValidationMessages.WorkereService.ServiceNotExist);

            RuleFor(b => b.BranchId)
                .MustAsync(
                    async (branchId, CancellationToken) =>
                        await branchRepository.IsAnyExistAsync(n => n.Id == branchId)
                )
                .WithMessage(ValidationMessages.WorkereService.BranchNotExist);

            RuleFor(command => command)
                .MustAsync(
                    async (command, CancellationToken) =>
                        !await workerServiceRepository.IsAnyExistAsync(n =>
                            n.WorkerId == command.WorkerId
                            && n.ServiceId == command.ServiceId
                            && n.BranchId == command.BranchId
                        )
                )
                .WithMessage(ValidationMessages.WorkereService.AssignWorkerToService);
        }
    }
}
