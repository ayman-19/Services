using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Application.Features.Bookings.Command.Update;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Queries.CalculateRate
{
    public class CalculateRateValidator : AbstractValidator<CalculateRateQuery>
    {
        private readonly IServiceProvider _serviceProvider;

        public CalculateRateValidator(IServiceProvider serviceProvider)
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
            RuleFor(b => b.WorkerId)
                .NotEmpty()
                .WithMessage(ValidationMessages.WorkerServices.WorkerIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.WorkerServices.WorkerIdIsRequired)
                .MustAsync(
                    async (id, cancellationToken) =>
                        await workerServiceRepository.IsAnyExistAsync(s => s.WorkerId == id)
                )
                .WithMessage(ValidationMessages.Workers.WorkerDoesNotExist);
        }
    }
}
