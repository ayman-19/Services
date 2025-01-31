using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Domain.Models;
using Services.Domain.Repositories;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;
using System;
using System.Net;

namespace Services.Application.Features.Users.Commands.Create
{
	public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseOf<CreateUserResult>>

	{
		private readonly IJWTManager _jwtManager;
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IEmailSender _emailSender;
		private readonly IPasswordHasher<User> _passwordHasher;

		public CreateUserCommandHandler(IJWTManager jwtManager, IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher, IEmailSender emailSender)
		{
			_jwtManager = jwtManager;
			_userRepository = userRepository;
			_unitOfWork = unitOfWork;
			_passwordHasher = passwordHasher;
			_emailSender = emailSender;
		}

		public async Task<ResponseOf<CreateUserResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
		{
			using(var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken))
			{
				User user = request;
				user.HashPassword(_passwordHasher, request.password);

				Token token = await _jwtManager.GenerateTokenAsync(user);
				user.Token = token;

				string code = await _jwtManager.GenerateCodeAsync();
				user.HashedCode(_passwordHasher, code);

				EntityEntry<User> result = await _userRepository.CreateAsync(user, cancellationToken);
				int success = await _unitOfWork.SaveChangesAsync(cancellationToken);

				if(success > 0)
				{
					await transaction.CommitAsync(cancellationToken);
					await _emailSender.SendEmailAsync(user.Email, ValidationMessages.User.ConfirmEmail, $"To Confirm Email Code: <h3>'{code}'</h3>");
					return new ResponseOf<CreateUserResult>
					{
						Message = ValidationMessages.Success,
						Success = true,
						StatusCode = (int)HttpStatusCode.OK,
						Result = user
					};
				}
				await transaction.RollbackAsync();
				throw new DatabaseTransactionException(ValidationMessages.Database.Error);
			}
		}
	}
}
