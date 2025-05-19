using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Branchs.Comands.Update
{
    public sealed class UpdateBranchValidator : AbstractValidator<UpdateBranchCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdateBranchValidator(IServiceProvider serviceProvider)
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
            RuleFor(s => s.langtuide)
                .NotEmpty()
                .WithMessage(ValidationMessages.Branchs.LongitudeIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Branchs.LongitudeIsRequired);

            RuleFor(s => s.latitude)
                .NotEmpty()
                .WithMessage(ValidationMessages.Branchs.LatitudeIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Branchs.LatitudeIsRequired);

            RuleFor(branch => branch.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Branchs.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Branchs.IdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await branchRepository.IsAnyExistAsync(branch => branch.Id == id)
                )
                .WithMessage(ValidationMessages.Branchs.BranchDoesNotExist);
        }
    }
}
