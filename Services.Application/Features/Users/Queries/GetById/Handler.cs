using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Enums;
using Services.Domain.Repositories;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Queries.GetById
{
    public sealed class GetUserHandler : IRequestHandler<GetUserQuery, ResponseOf<object>>
    {
        private readonly IUserRepository _userRepository;

        public GetUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResponseOf<object>> Handle(
            GetUserQuery request,
            CancellationToken cancellationToken
        )
        {
            var user = await _userRepository.GetAsync(
                user => user.Id == request.id,
                user => user,
                user =>
                    user.Include(ur => ur.UserRoles)
                        .ThenInclude(ur => ur.Role)
                        .Include(w => w.Worker)
                        .Include(c => c.Customer),
                false,
                cancellationToken
            );

            var roles = user.UserRoles.Select(role => role.Role.Name);

            object result = user.UserType switch
            {
                UserType.Worker => new GetWorkerUserResult(
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Phone,
                    user.CreateOn,
                    roles,
                    user.Worker.Experience,
                    user.Worker.Status
                ),
                UserType.Customer => new GetCustomerUserResult(
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Phone,
                    user.CreateOn,
                    roles
                ),
                _ => throw new NotSupportedException($"Unsupported user type: {user.UserType}"),
            };

            return new ResponseOf<object>
            {
                Success = true,
                Message = ValidationMessages.Success,
                StatusCode = (int)HttpStatusCode.OK,
                Result = result,
            };
        }
    }
}
