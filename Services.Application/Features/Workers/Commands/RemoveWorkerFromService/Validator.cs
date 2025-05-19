using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Commands.RemoveWorkerFromService
{
    public sealed class RemoveWorkerFromServiceValidator
        : AbstractValidator<RemoveWorkerFromServiceCommand>
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
                .WithMessage(ValidationMessages.WorkerServices.WorkerIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.WorkerServices.WorkerIdIsRequired);

            RuleFor(s => s.ServiceId)
                .NotEmpty()
                .WithMessage(ValidationMessages.WorkerServices.ServiceIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.WorkerServices.ServiceIdIsRequired);

            RuleFor(command => command)
                .MustAsync(
                    async (command, CancellationToken) =>
                        await workerServiceRepository.IsAnyExistAsync(n =>
                            n.WorkerId == command.WorkerId && n.ServiceId == command.ServiceId
                        )
                )
                .WithMessage(ValidationMessages.WorkerServices.WorkerNotAssignedToService);
        }
    }
}
