using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Branchs.Queries.GetById
{
    public sealed class GetBranchValidator : AbstractValidator<GetBranchQuery>
    {
        private readonly IServiceProvider _serviceProvider;

        public GetBranchValidator(IServiceProvider serviceProvider)
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
                .WithMessage(ValidationMessages.Branchs.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Branchs.IdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await branchRepository.IsAnyExistAsync(branch => branch.Id == id)
                )
                .WithMessage(ValidationMessages.Branchs.IdIsNotFound);
        }
    }
}
