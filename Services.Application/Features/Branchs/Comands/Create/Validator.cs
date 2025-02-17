using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Application.Features.Services.Commands.Create;
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
                .WithMessage(ValidationMessages.Branch.NameIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Branch.NameIsRequired);
            RuleFor(s => s.langtuide)
                .NotEmpty()
                .WithMessage(ValidationMessages.Branch.Langtuide)
                .NotNull()
                .WithMessage(ValidationMessages.Branch.LangtuideCantBeNull);
            RuleFor(s => s.latitude)
                .NotEmpty()
                .WithMessage(ValidationMessages.Branch.Latitude)
                .NotNull()
                .WithMessage(ValidationMessages.Branch.LatitudeCantBeNull);

            RuleFor(s => s.name)
                .MustAsync(
                    async (name, CancellationToken) =>
                        !await branchRepository.IsAnyExistAsync(n => n.Name == name)
                )
                .WithMessage(ValidationMessages.Branch.NameIsExist);
        }
    }
}
