using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Application.Features.Services.Commands.Create;
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
                .WithMessage(ValidationMessages.Service.NameIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Service.NameIsRequired);

            RuleFor(s => s.id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Service.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Service.IdIsRequired);

            RuleFor(s => s.description)
                .NotEmpty()
                .WithMessage(ValidationMessages.Service.DescriptionIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Service.DescriptionIsRequired);

            RuleFor(s => s.id)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await serviceRepository.IsAnyExistAsync(n => n.Id == id)
                )
                .WithMessage(ValidationMessages.Service.ServiceNotExist);

            RuleFor(s => s)
                .MustAsync(
                    async (request, CancellationToken) =>
                        !await serviceRepository.IsAnyExistAsync(n =>
                            n.Name == request.name && n.Id != request.id
                        )
                )
                .WithMessage(ValidationMessages.Service.NameIsExist);
        }
    }
}
