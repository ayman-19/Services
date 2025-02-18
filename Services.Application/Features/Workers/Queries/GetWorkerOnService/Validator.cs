using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Application.Features.Workers.Queries.GetWorkerOnService;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Workers.Queries.GetWorkersOnService
{
    public sealed class GetWorkerOnServiceValidator : AbstractValidator<GetWorkerOnServiceQuery>
    {
        private readonly IServiceProvider _serviceProvider;

        public GetWorkerOnServiceValidator(IServiceProvider serviceProvider)
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

            RuleFor(s => s.BranchId)
                .NotEmpty()
                .WithMessage(ValidationMessages.WorkereService.BranchIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.WorkereService.BranchIdIsRequired);

            RuleFor(query => query)
                .MustAsync(
                    async (query, CancellationToken) =>
                        await workerServiceRepository.IsAnyExistAsync(n =>
                            n.WorkerId == query.WorkerId
                            && n.ServiceId == query.ServiceId
                            && n.BranchId == query.BranchId
                        )
                )
                .WithMessage(ValidationMessages.WorkereService.WorkerNotAssignToService);
        }
    }
}
