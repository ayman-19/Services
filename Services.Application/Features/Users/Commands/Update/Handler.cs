using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Services.Domain.Abstraction;
using Services.Domain.Models;
using Services.Domain.Repositories;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.Update
{
    public sealed class UpdateUserHandler
        : IRequestHandler<UpdateUserCommand, ResponseOf<UpdateUserResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly ITokenRepository _tokenRepository;
        private readonly IJWTManager _jwtManager;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UpdateUserHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IEmailSender emailSender,
            ITokenRepository tokenRepository,
            IJWTManager jwtManager,
            IPasswordHasher<User> passwordHasher
        )
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _tokenRepository = tokenRepository;
            _jwtManager = jwtManager;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResponseOf<UpdateUserResult>> Handle(
            UpdateUserCommand request,
            CancellationToken cancellationToken
        )
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync(
                cancellationToken
            );

            try
            {
                var user = await _userRepository.GetByIdAsync(request.id);

                var emailChanged = !string.Equals(
                    user.Email,
                    request.email,
                    StringComparison.OrdinalIgnoreCase
                );

                user.Update(request.name, request.email, request.phone, !emailChanged);

                await _tokenRepository.DeleteByUserIdAsync(user.Id);
                var verificationCode = await _jwtManager.GenerateCodeAsync();
                var newToken = await _jwtManager.GenerateTokenAsync(user, cancellationToken);
                user.HashedCode(_passwordHasher, verificationCode);
                user.Token = newToken;

                await _userRepository.UpdateAsync(user, cancellationToken);
                var saved = await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                if (saved > 0)
                {
                    if (emailChanged)
                    {
                        await _emailSender.SendEmailAsync(
                            user.Email,
                            ValidationMessages.Users.ConfirmEmail,
                            $"To confirm your email, use this code: <h3>'{verificationCode}'</h3>"
                        );
                    }

                    return new ResponseOf<UpdateUserResult>
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = user,
                    };
                }
                return new ResponseOf<UpdateUserResult>
                {
                    Message = ValidationMessages.Failure,
                    Success = false,
                    StatusCode = (int)HttpStatusCode.ExpectationFailed,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new Exception($"UpdateUser failed: {ex.Message}", ex);
            }
        }
    }
}
