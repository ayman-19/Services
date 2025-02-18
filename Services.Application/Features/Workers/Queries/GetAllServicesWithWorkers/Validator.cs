﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Queries.GetAllServicesWithWorkers
{
    public sealed class GetAllServicesWithWorkersValidator
        : AbstractValidator<GetAllServicesWithWorkersQuery>
    {
        private readonly IServiceProvider _serviceProvider;

        public GetAllServicesWithWorkersValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IWorkerServiceRepository>());
        }

        private void ValidateRequest(IWorkerServiceRepository workerServiceRepository)
        {
            RuleFor(worker => worker.WorkerId)
                .NotEmpty()
                .WithMessage(ValidationMessages.WorkereService.WorkerIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.WorkereService.WorkerIdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await workerServiceRepository.IsAnyExistAsync(worker => worker.Id == id)
                )
                .WithMessage(ValidationMessages.WorkereService.WorkerNotExist);
        }
    }
}
