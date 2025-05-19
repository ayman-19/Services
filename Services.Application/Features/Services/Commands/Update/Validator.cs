using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Services.Commands.Update
{
    public sealed class UpdateServiceValidator : AbstractValidator<UpdateServiceCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdateServiceValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IServiceRepository>());
        }

        private void ValidateRequest(IServiceRepository serviceRepository)
        {
            RuleFor(s => s.name)
                .NotEmpty()
                .WithMessage(ValidationMessages.Services.NameIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Services.NameIsRequired);

            RuleFor(s => s.id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Services.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Services.IdIsRequired);

            RuleFor(s => s.description)
                .NotEmpty()
                .WithMessage(ValidationMessages.Services.DescriptionIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Services.DescriptionIsRequired);

            RuleFor(s => s.id)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await serviceRepository.IsAnyExistAsync(n => n.Id == id)
                )
                .WithMessage(ValidationMessages.Services.ServiceDoesNotExist);

            RuleFor(s => s)
                .MustAsync(
                    async (request, CancellationToken) =>
                        !await serviceRepository.IsAnyExistAsync(n =>
                            n.Name == request.name && n.Id != request.id
                        )
                )
                .WithMessage(ValidationMessages.Services.NameExists);
        }
    }
}
