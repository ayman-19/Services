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
                .WithMessage(ValidationMessages.Services.NameIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Services.NameIsRequired);
            RuleFor(s => s.description)
                .NotEmpty()
                .WithMessage(ValidationMessages.Services.DescriptionIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Services.DescriptionIsRequired);

            //RuleFor(s => s.File)
            //    .NotEmpty()
            //    .WithMessage(ValidationMessages.Services.ImageIsRequired)
            //    .NotNull()
            //    .WithMessage(ValidationMessages.Services.ImageIsRequired)
            //    .Must((file) => file.Length == 0 ? false : true)
            //    .WithMessage(ValidationMessages.Services.ImageIsRequired);

            RuleFor(s => s.name)
                .MustAsync(
                    async (name, CancellationToken) =>
                        !await serviceRepository.IsAnyExistAsync(n => n.Name == name)
                )
                .WithMessage(ValidationMessages.Services.NameExists);
        }
    }
}
