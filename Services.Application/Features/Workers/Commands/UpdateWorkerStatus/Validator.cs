using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Application.Features.Workers.Commands.UpdateWorkerOnServiceAvailabilty;
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
                .WithMessage(ValidationMessages.WorkereService.WorkerIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.WorkereService.WorkerIdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await workerRepository.IsAnyExistAsync(n => n.UserId == id)
                )
                .WithMessage(ValidationMessages.WorkereService.WorkerNotExist);
        }
    }
}
