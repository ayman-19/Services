using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Commands.UpdateWorkerOnServiceAvailabilty
{
    public sealed class UpdateWorkerOnServiceAvailabiltyValidator
        : AbstractValidator<UpdateWorkerOnServiceAvailabiltyCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdateWorkerOnServiceAvailabiltyValidator(IServiceProvider serviceProvider)
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
