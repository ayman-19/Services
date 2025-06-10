using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Domain.Enums;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Queries.GetWorkersOnService
{
    public class GetWorkersOnServiceValidator : AbstractValidator<GetWorkersOnServiceQuery>
    {
        private readonly IServiceProvider _serviceProvider;

        public GetWorkersOnServiceValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(
                scope.ServiceProvider.GetRequiredService<IServiceRepository>(),
                scope.ServiceProvider.GetRequiredService<IWorkerServiceRepository>()
            );
        }

        private void ValidateRequest(
            IServiceRepository serviceRepository,
            IWorkerServiceRepository workerServiceRepository
        )
        {
            RuleFor(s => s.ServiceId)
                .NotEmpty()
                .WithMessage(ValidationMessages.WorkerServices.ServiceIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.WorkerServices.ServiceIdIsRequired);

            RuleFor(query => query.ServiceId)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await serviceRepository.IsAnyExistAsync(n => n.Id == id)
                )
                .WithMessage(ValidationMessages.WorkerServices.ServiceDoesNotExist);

            RuleFor(query => query.Status)
                .MustAsync(
                    async (status, CancellationToken) =>
                        await workerServiceRepository.IsAnyExistAsync(ws =>
                            status == null
                                ? ws.Worker.Status == Status.Active
                                : ws.Worker.Status == status
                        )
                )
                .WithMessage(ValidationMessages.WorkerServices.NotFound);
        }
    }
}
