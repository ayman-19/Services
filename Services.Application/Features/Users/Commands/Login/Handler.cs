using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Services.Domain.Abstraction;
using Services.Domain.Models;
using Services.Domain.Repositories;
using Services.Shared.Enums;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.Login
{
    public sealed class LoginUserHandler : IRequestHandler<LoginUserCommand, Response>
    {
        private readonly IJWTManager _jwtManager;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LoginUserHandler(
            IJWTManager jwtManager,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher<User> passwordHasher,
            IEmailSender emailSender
        )
        {
            _jwtManager = jwtManager;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _emailSender = emailSender;
        }

        public async Task<Response> Handle(
            LoginUserCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    User user;

                    if (request.type == LoginType.Email)
                    {
                        user = await _userRepository.GetByEmailAsync(request.emailOrPhone);

                        if (!user.ConfirmAccount)
                            throw new InvalidException(ValidationMessages.User.EmailNotConfirmed);

                        if (!VerifyPassword(user, user.HashedPassword, request.password))
                            throw new InvalidException(ValidationMessages.User.IncorrectPassword);
                    }
                    else if (request.type == LoginType.Phone)
                    {
                        user = await _userRepository.GetByPhoneAsync(request.emailOrPhone);

                        if (!user.ConfirmAccount)
                            throw new InvalidException(ValidationMessages.User.EmailNotConfirmed);

                        if (!VerifyPassword(user, user.HashedPassword, request.password))
                            throw new InvalidException(ValidationMessages.User.IncorrectPassword);
                    }
                    else
                        throw new InvalidException(ValidationMessages.User.MakeSureInformation);

                    return new ResponseOf<LoginUserResult>
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = await _jwtManager.LoginAsync(user),
                    };
                }
                catch (Exception)
                {
                    throw new DatabaseTransactionException(ValidationMessages.Database.Error);
                }
            }
        }

        private bool VerifyPassword(User user, string hashedPassword, string password) =>
            _passwordHasher.VerifyHashedPassword(user, hashedPassword, password)
            == PasswordVerificationResult.Success;
    }
}
