using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Branchs.Queries.GetByUserId
{
    public sealed class GetBranchByUserIdValidator : AbstractValidator<GetBranchByUserIdQuery>
    {
        private readonly IServiceProvider _serviceProvider;

        public GetBranchByUserIdValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IBranchRepository>());
        }

        private void ValidateRequest(IBranchRepository branchRepository)
        {
            RuleFor(branch => branch.UserId)
                .NotEmpty()
                .WithMessage(ValidationMessages.Branchs.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Branchs.IdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await branchRepository.IsAnyExistAsync(branch => branch.UserId == id)
                )
                .WithMessage(ValidationMessages.Branchs.BranchDoesNotExist);
        }
    }
}
