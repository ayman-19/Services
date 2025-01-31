using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Services.Domain.Abstraction;
using Services.Domain.Models;
using Services.Domain.Repositories;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;
using System.Net;

namespace Services.Application.Features.Users.Commands.Login
{
	public sealed class LoginUserHandler : IRequestHandler<LoginUserCommand, Response>
	{
		private readonly IJWTManager _jwtManager;
		private readonly IUserRepository _userRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IEmailSender _emailSender;
		private readonly IPasswordHasher<string> _passwordHasher;

		public LoginUserHandler(IJWTManager jwtManager, IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher<string> passwordHasher, IEmailSender emailSender)
		{
			_jwtManager = jwtManager;
			_userRepository = userRepository;
			_unitOfWork = unitOfWork;
			_passwordHasher = passwordHasher;
			_emailSender = emailSender;
		}
		public async Task<Response> Handle(LoginUserCommand request, CancellationToken cancellationToken)
		{
			using (var transaction = await _unitOfWork.BeginTransactionAsync())
			{
				try
				{
					User user = await _userRepository.GetByEmailAsync(request.emailOrPhone);
					if (user != null && user.ConfirmAccount)
					{
						if (!VerifyPassword(user.HashedPassword, request.password))
							throw new InvalidException(ValidationMessages.User.IncorrectPassword);

						return new ResponseOf<LoginUserResult>
						{
							Message = ValidationMessages.Success,
							Success = true,
							StatusCode = (int)HttpStatusCode.OK,
							Result = await _jwtManager.LoginAsync(user)
						};
					}


					user = await _userRepository.GetByPhoneAsync(request.emailOrPhone);
					if (user != null && user.ConfirmAccount)
					{
						if (!VerifyPassword(user.HashedPassword, request.password))
							throw new InvalidException(ValidationMessages.User.IncorrectPassword);

						return new ResponseOf<LoginUserResult>
						{
							Message = ValidationMessages.Success,
							Success = true,
							StatusCode = (int)HttpStatusCode.OK,
							Result = await _jwtManager.LoginAsync(user)
						};
					}
					await transaction.CommitAsync();
				}
				catch (Exception)
				{
					await transaction.RollbackAsync();
					throw new DatabaseTransactionException(ValidationMessages.Database.Error);
				}
			}
			return new Response
			{
				Message = ValidationMessages.Falier,
				Success = false,
				StatusCode = (int)HttpStatusCode.Conflict,
			};
		}

		private bool VerifyPassword(string hashedPassword,  string password)
			=> _passwordHasher.VerifyHashedPassword(null!, hashedPassword, password) == PasswordVerificationResult.Success;
	}
}
