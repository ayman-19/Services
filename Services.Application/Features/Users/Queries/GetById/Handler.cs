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
            //    var user = await _userRepository.GetAsync(
            //        user => user.Id == request.id,
            //        user => user,
            //        user =>
            //            user.Include(ur => ur.UserRoles)
            //                .ThenInclude(ur => ur.Role)
            //                .Include(w => w.Worker)
            //                .Include(c => c.Customer)
            //                .ThenInclude(p => p.Point),
            //        false,
            //        cancellationToken
            //    );

            //    var roles = user.UserRoles.Select(role => role.Role.Name);

            //    object result = user.UserType switch
            //    {
            //        UserType.Worker => new GetWorkerUserResult(
            //            user.Id,
            //            user.Name,
            //            user.Email,
            //            user.Phone,
            //            user.CreateOn,
            //            roles,
            //            user.Worker.Experience,
            //            user.Worker.Status
            //        ),
            //        UserType.Customer => new GetCustomerUserResult(
            //            user.Id,
            //            user.Name,
            //            user.Email,
            //            user.Phone,
            //            user.CreateOn,
            //            roles,
            //            user.Customer.Point.Number
            //        ),
            //        _ => throw new NotSupportedException($"Unsupported user type: {user.UserType}"),
            //    };


            var user = await _userRepository.GetAsync(
                user => user.Id == request.id,
                user => new
                {
                    User = user,
                    Roles = user.UserRoles != null
                        ? user.UserRoles.Select(ur => ur.Role != null ? ur.Role.Name : null)
                        : Enumerable.Empty<string>(),
                    Worker = user.UserType == UserType.Worker && user.Worker != null
                        ? new { user.Worker.Experience, user.Worker.Status }
                        : new { Experience = 0.0, Status = Status.InActive },
                    Customer = user.UserType == UserType.Customer
                    && user.Customer != null
                    && user.Customer.Point != null
                        ? new { user.Customer.Point.Number }
                        : new { Number = 0 },
                    user.UserType,
                },
                query =>
                    query
                        .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                        .Include(u => u.Worker)
                        .Include(u => u.Customer)
                        .ThenInclude(c => c.Point),
                false,
                cancellationToken: cancellationToken
            );

            object result = user.UserType switch
            {
                UserType.Worker => new GetWorkerUserResult(
                    user.User.Id,
                    user.User.Name,
                    user.User.Email,
                    user.User.Phone,
                    user.User.CreateOn,
                    user.Roles,
                    user.Worker.Experience,
                    user.Worker.Status
                ),
                UserType.Customer => new GetCustomerUserResult(
                    user.User.Id,
                    user.User.Name,
                    user.User.Email,
                    user.User.Phone,
                    user.User.CreateOn,
                    user.Roles,
                    user.Customer.Number
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
