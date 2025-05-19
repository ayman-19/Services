using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Services.Queries.GetById
{
    public sealed class GetServiceValidator : AbstractValidator<GetServiceQuery>
    {
        private readonly IServiceProvider _serviceProvider;

        public GetServiceValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IServiceRepository>());
        }

        private void ValidateRequest(IServiceRepository serviceRepository)
        {
            RuleFor(service => service.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Services.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Services.IdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await serviceRepository.IsAnyExistAsync(service => service.Id == id)
                )
                .WithMessage(ValidationMessages.Services.ServiceDoesNotExist);
        }
    }
}
