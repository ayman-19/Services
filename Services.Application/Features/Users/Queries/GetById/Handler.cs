using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Repositories;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Features.Users.Queries.GetById
{
	public sealed class GetUserHandler : IRequestHandler<GetUserQuery, ResponseOf<GetUserResult>>
	{
		private readonly IUserRepository _userRepository;

		public GetUserHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<ResponseOf<GetUserResult>> Handle(GetUserQuery request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetAsync(user => user.Id == request.id, user => new GetUserResult(user.Id, user.Name, user.Email, user.CreateOn, user.UserRoles.Select(role => role.Role.Name)), user => user.Include(ur => ur.UserRoles).ThenInclude(ur => ur.Role), false, cancellationToken);
			return new ResponseOf<GetUserResult>
			{
				Message = ValidationMessages.Success,
				Success = true,
				StatusCode = (int)HttpStatusCode.OK,
				Result = user
			};
		}
	}
}
