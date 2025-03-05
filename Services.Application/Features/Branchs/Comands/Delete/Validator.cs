using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Branchs.Comands.Delete
{
    public sealed class DeleteBranchValidator : AbstractValidator<DeleteBranchCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public DeleteBranchValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IBranchRepository>());
        }

        private void ValidateRequest(IBranchRepository branchRepository)
        {
            RuleFor(branch => branch.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Branch.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Branch.IdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await branchRepository.IsAnyExistAsync(branch => branch.Id == id)
                )
                .WithMessage(ValidationMessages.Branch.BranchNotExist);
        }
    }
}
