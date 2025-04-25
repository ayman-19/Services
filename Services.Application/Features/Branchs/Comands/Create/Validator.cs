using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Branchs.Comands.Create
{
    public sealed class CreateBranchValidator : AbstractValidator<CreateBranchCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public CreateBranchValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IBranchRepository>());
        }

        private void ValidateRequest(IBranchRepository branchRepository)
        {
            RuleFor(s => s.name)
                .NotEmpty()
                .WithMessage(ValidationMessages.Branchs.NameIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Branchs.NameIsRequired);

            RuleFor(s => s.description)
                .NotEmpty()
                .WithMessage(ValidationMessages.Branchs.DescriptionIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Branchs.DescriptionIsRequired);

            RuleFor(s => s.langtuide)
                .NotEmpty()
                .WithMessage(ValidationMessages.Branchs.Langtuide)
                .NotNull()
                .WithMessage(ValidationMessages.Branchs.Langtuide);
            RuleFor(s => s.latitude)
                .NotEmpty()
                .WithMessage(ValidationMessages.Branchs.Latitude)
                .NotNull()
                .WithMessage(ValidationMessages.Branchs.Latitude);
        }
    }
}
