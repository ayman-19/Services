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
            RuleFor(b => b.name)
                .NotEmpty()
                .WithMessage(ValidationMessages.Branch.NameIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Branch.NameIsRequired);

            RuleFor(b => b.id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Branch.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Branch.IdIsRequired);

            RuleFor(s => s.langtuide)
                .NotEmpty()
                .WithMessage(ValidationMessages.Branch.Langtuide)
                .NotNull()
                .WithMessage(ValidationMessages.Branch.Langtuide);

            RuleFor(b => b.id)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await branchRepository.IsAnyExistAsync(n => n.Id == id)
                )
                .WithMessage(ValidationMessages.Service.ServiceNotExist);

            RuleFor(b => b)
                .MustAsync(
                    async (request, CancellationToken) =>
                        !await branchRepository.IsAnyExistAsync(n =>
                            n.Name == request.name && n.Id != request.id
                        )
                )
                .WithMessage(ValidationMessages.Service.NameIsExist);
        }
    }
}
