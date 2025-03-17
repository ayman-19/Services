using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Services.Commands.Delete
{
    public sealed class DeleteServiceValidator : AbstractValidator<DeleteServiceCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteServiceValidator(IServiceProvider serviceProvider)
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
                .WithMessage(ValidationMessages.Service.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Service.IdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await serviceRepository.IsAnyExistAsync(service => service.Id == id)
                )
                .WithMessage(ValidationMessages.Service.ServiceNotExist);
        }
    }
}
