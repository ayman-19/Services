using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Application.Features.Services.Queries.GetById;
using Services.Domain.Abstraction;
using Services.Shared.ValidationMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Features.Branchs.Queries.GetById
{

    public sealed class GetBranchValidator:AbstractValidator<GetBranchQuery>
    {
        private readonly IServiceProvider _serviceProvider;

        public GetBranchValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(scope.ServiceProvider.GetRequiredService<IServiceRepository>());
        }

        private void ValidateRequest(IServiceRepository serviceRepository)
        {
            RuleFor(branch => branch.Id)
                .NotEmpty()
                .WithMessage(ValidationMessages.Branch.IdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Branch.IdIsRequired)
                .MustAsync(
                    async (id, CancellationToken) =>
                        await serviceRepository.IsAnyExistAsync(branch => branch.Id == id)
                )
                .WithMessage(ValidationMessages.Branch.IdIsNotFound);
        }
    }
}
