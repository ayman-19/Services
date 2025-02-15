using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Application.Features.Workers.Commands.AssignWorkerToService;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Commands.RemoveWorkerFromService
{
    public sealed class RemoveWorkerFromServiceValidator
        : AbstractValidator<AssignWorkerToServiceCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public RemoveWorkerFromServiceValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IWorkerServiceRepository>());
        }

        private void ValidateRequest(IWorkerServiceRepository workerServiceRepository)
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

            RuleFor(command => command)
                .MustAsync(
                    async (command, CancellationToken) =>
                        await workerServiceRepository.IsAnyExistAsync(n =>
                            n.WorkerId == command.WorkerId && n.ServiceId == command.ServiceId
                        )
                )
                .WithMessage(ValidationMessages.WorkereService.WorkerNotAssignToService);
        }
    }
}
