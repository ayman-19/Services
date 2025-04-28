using System.Net;
using MediatR;
using Services.Application.Features.Users.Queries.GetPermissions;
using Services.Domain.Repositories;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Queries.GetRoles
{
    public sealed record GetRolesHandler(IRoleRepository _roleRepository)
        : IRequestHandler<GetRolesQuery, ResponseOf<IReadOnlyCollection<GetRolesResult>>>
    {
        public async Task<ResponseOf<IReadOnlyCollection<GetRolesResult>>> Handle(
            GetRolesQuery request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                IReadOnlyCollection<GetRolesResult> permissions = await _roleRepository.GetAllAsync(
                    selector => new GetRolesResult(selector.Id, selector.Name),
                    cancellationToken: cancellationToken
                );
                return new ResponseOf<IReadOnlyCollection<GetRolesResult>>
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
