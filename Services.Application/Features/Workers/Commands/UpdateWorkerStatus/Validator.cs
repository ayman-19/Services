using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Commands.UpdateWorkerStatus
{
    public sealed class UpdateWorkerStatusValidator : AbstractValidator<UpdateWorkerStatusCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdateWorkerStatusValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IWorkerRepository>());
        }

        private void ValidateRequest(IWorkerRepository workerRepository)
        {
            RuleFor(command => command.WorkerId)
                .NotEmpty()
                .WithMessage(ValidationMessages.WorkerServices.WorkerIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.WorkerServices.WorkerIdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await workerRepository.IsAnyExistAsync(n => n.UserId == id)
                )
                .WithMessage(ValidationMessages.WorkerServices.WorkerDoesNotExist);
        }
    }
}
