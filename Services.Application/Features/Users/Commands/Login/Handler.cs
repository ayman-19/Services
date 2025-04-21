using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.Domain.Abstraction;
using Services.Domain.Models;
using Services.Domain.Repositories;
using Services.Shared.Enums;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.Login
{
    public sealed class LoginUserHandler
        : IRequestHandler<LoginUserCommand, ResponseOf<LoginUserResult>>
    {
        private readonly IJWTManager _jwtManager;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJobs _jobs;

        public LoginUserHandler(
            IJWTManager jwtManager,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher<User> passwordHasher,
            IJobs jobs
        )
        {
            _jwtManager = jwtManager;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _jobs = jobs;
        }

        public async Task<ResponseOf<LoginUserResult>> Handle(
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
                        user = await _userRepository.GetByEmailAsync(request.emailOrPhone);
                    else if (request.type == LoginType.Phone)
                        user = await _userRepository.GetByPhoneAsync(request.emailOrPhone);
                    else
                        throw new InvalidException(ValidationMessages.Users.MakeSureInformation);

                    if (!user.ConfirmAccount)
                    {
                        string code = await _jwtManager.GenerateCodeAsync();
                        await _userRepository.UpdateCodeAsync(user.Id, code);
                        await transaction.CommitAsync(cancellationToken);
                        await _jobs.SendEmailByJobAsync(
                            user.Email,
                            $"To Confirm Email Code: <h3>{code}</h3>"
                        );
                        return new ResponseOf<LoginUserResult>
                        {
                            Message = ValidationMessages.Users.EmailNotConfirmed,
                            StatusCode = 499,
                            Success = false,
                        };
                    }

                    if (!VerifyPassword(user, user.HashedPassword, request.password))
                        throw new InvalidException(ValidationMessages.Users.IncorrectPassword);

                    await _userRepository.UpdateBranchAsync(
                        user.Id,
                        request.Latitude,
                        request.Longitude
                    );

                    return new ResponseOf<LoginUserResult>
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = await _jwtManager.LoginAsync(user),
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new DatabaseTransactionException(ex.Message);
                }
            }
        }

        private bool VerifyPassword(User user, string hashedPassword, string password) =>
            _passwordHasher.VerifyHashedPassword(user, hashedPassword, password)
            == PasswordVerificationResult.Success;
    }
}
