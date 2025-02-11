using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Domain.Repositories;
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
        }

      
    }
}
