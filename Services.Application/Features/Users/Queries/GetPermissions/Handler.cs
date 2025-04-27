using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Queries.GetPermissions
{
    public sealed record GetPermissionsHandler(IPermissionRepository _permissionRepository)
        : IRequestHandler<
            GetPermissionsQuery,
            ResponseOf<IReadOnlyCollection<GetPermissionsResult>>
        >
    {
        public async Task<ResponseOf<IReadOnlyCollection<GetPermissionsResult>>> Handle(
            GetPermissionsQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                IReadOnlyCollection<GetPermissionsResult> permissions =
                    await _permissionRepository.GetAllAsync(
                        selector => new GetPermissionsResult(selector.Id, selector.Name),
                        cancellationToken: cancellationToken
                    );
                return new ResponseOf<IReadOnlyCollection<GetPermissionsResult>>
                {
                    Message = ValidationMessages.Success,
                    Success = true,
                    StatusCode = (int)HttpStatusCode.OK,
                    Result = permissions,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
