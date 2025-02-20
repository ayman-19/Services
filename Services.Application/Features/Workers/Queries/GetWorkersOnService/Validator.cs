using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
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
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IWorkerServiceRepository>());
        }

        private void ValidateRequest(IWorkerServiceRepository workerServiceRepository)
        {
            RuleFor(s => s.ServiceId)
                .NotEmpty()
                .WithMessage(ValidationMessages.WorkereService.ServiceIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.WorkereService.ServiceIdIsRequired);

            RuleFor(query => query)
                .MustAsync(
                    async (query, CancellationToken) =>
                        await workerServiceRepository.IsAnyExistAsync(n =>
                            n.ServiceId == query.ServiceId
                        )
                )
                .WithMessage(ValidationMessages.WorkereService.ServiceNotExist);
        }
    }
}
