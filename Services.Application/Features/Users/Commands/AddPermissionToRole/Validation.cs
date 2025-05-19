using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Services.Domain.Abstraction;
using Services.Domain.Repositories;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.AddPermissionToRole
{
    public class AddPermissionToRoleValidator : AbstractValidator<AddPermissionToRoleCommand>
    {
        private readonly IServiceProvider _serviceProvider;

        public AddPermissionToRoleValidator(IServiceProvider serviceProvider)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;
            _serviceProvider = serviceProvider;
            var scope = _serviceProvider.CreateScope();
            ValidateRequest(
                scope.ServiceProvider.GetRequiredService<IRolePermissionRepository>(),
                scope.ServiceProvider.GetRequiredService<IRoleRepository>(),
                scope.ServiceProvider.GetRequiredService<IPermissionRepository>()
            );
        }

        private void ValidateRequest(
            IRolePermissionRepository rolePermissionRepository,
            IRoleRepository roleRepository,
            IPermissionRepository permissionRepository
        )
        {
            RuleFor(x => x.RoleId)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.RoleIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.RoleIdIsRequired);

            RuleFor(x => x.PermissionId)
                .NotEmpty()
                .WithMessage(ValidationMessages.Users.PermissionIdIsRequired)
                .NotNull()
                .WithMessage(ValidationMessages.Users.PermissionIdIsRequired);

            RuleFor(x => x.RoleId)
                .MustAsync(
                    async (id, cancellationToken) =>
                        await roleRepository.IsAnyExistAsync(r => r.Id == id)
                )
                .WithMessage(ValidationMessages.Users.RoleDoesNotExist);

            RuleFor(x => x.PermissionId)
                .MustAsync(
                    async (id, cancellationToken) =>
                        await permissionRepository.IsAnyExistAsync(r => r.Id == id)
                )
                .WithMessage(ValidationMessages.Users.PermissionDoesNotExist);

            RuleFor(x => x)
                .MustAsync(
                    async (ids, cancellationToken) =>
                        !await rolePermissionRepository.IsAnyExistAsync(r =>
                            r.RoleId == ids.RoleId && r.PermissionId == ids.PermissionId
                        )
                )
                .WithMessage(ValidationMessages.Users.PermissionAlreadyAssignedToRole);
        }
    }
}
