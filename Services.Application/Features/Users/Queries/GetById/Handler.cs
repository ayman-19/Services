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
                user => new
                {
                    User = user,
                    Roles = user.UserRoles != null
                        ? user.UserRoles.Select(ur => ur.Role != null ? ur.Role.Name : null)
                        : Enumerable.Empty<string>(),
                    Worker = user.UserType == UserType.Worker
                    && user.Worker != null
                    && user.Worker.WorkerServices.Any()
                        ? new
                        {
                            Available = user
                                .Worker.WorkerServices.Select(av => av.Availabilty)
                                .First(),
                            user.Worker.Experience,
                            user.Worker.Status,
                        }
                        : new
                        {
                            Available = false,
                            Experience = 0.0,
                            Status = Status.InActive,
                        },
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
                        .ThenInclude(ws => ws.WorkerServices)
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
                    user.Worker.Status,
                    user.Worker.Available
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
