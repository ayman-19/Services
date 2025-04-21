using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Services.Commands.Create
{
    public sealed class CreateServiceValidator : AbstractValidator<CreateServiceCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public CreateServiceValidator(IServiceProvider serviceProvider)
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
                .WithMessage(ValidationMessages.Service.NameIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Service.NameIsRequired);
            RuleFor(s => s.description)
                .NotEmpty()
                .WithMessage(ValidationMessages.Service.DescriptionIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Service.DescriptionIsRequired);

            RuleFor(s => s.File)
                .NotEmpty()
                .WithMessage(ValidationMessages.Service.ImageIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Service.ImageIsRequired)
                .Must((file) => file.Length == 0 ? false : true)
                .WithMessage(ValidationMessages.Service.ImageIsRequired);

            RuleFor(s => s.name)
                .MustAsync(
                    async (name, CancellationToken) =>
                        !await serviceRepository.IsAnyExistAsync(n => n.Name == name)
                )
                .WithMessage(ValidationMessages.Service.NameIsExist);
        }
    }
}
