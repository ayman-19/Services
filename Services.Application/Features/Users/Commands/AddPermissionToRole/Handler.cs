using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Domain.Models;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.AddPermissionToRole
{
    public sealed record AddPermissionToRoleHandler(
        IRolePermissionRepository _RolePermissionRepository,
        IUnitOfWork _unitOfWork
    ) : IRequestHandler<AddPermissionToRoleCommand, Response>
    {
        public async Task<Response> Handle(
            AddPermissionToRoleCommand request,
            CancellationToken cancellationToken
        )
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                RolePermission rolePermission = request;
                await _RolePermissionRepository.CreateAsync(rolePermission);
                await _RolePermissionRepository.DeleteTokenForUsersAssignThisRole(
                    request.RoleId,
                    cancellationToken
                );

                var success = await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return new()
                {
                    Message = ValidationMessages.Success,
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
