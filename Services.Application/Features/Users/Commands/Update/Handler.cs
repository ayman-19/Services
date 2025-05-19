using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Services.Domain.Abstraction;
using Services.Domain.Models;
using Services.Domain.Repositories;
using Services.Shared.Exceptions;
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
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    User user = await _userRepository.GetByIdAsync(request.id);
                    user.Update(request.name, request.email, request.phone);
                    await _tokenRepository.DeleteByUserIdAsync(user.Id);
                    string code = await _jwtManager.GenerateCodeAsync();
                    user.HashedCode(_passwordHasher, code);

                    await _userRepository.UpdateAsync(user);
                    int success = await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();

                    if (success > 0)
                    {
                        await _emailSender.SendEmailAsync(
                            user.Email,
                            ValidationMessages.Users.ConfirmEmail,
                            $"To Confirm Email Code: <h3>'{code}'</h3>"
                        );

                        return new ResponseOf<UpdateUserResult>
                        {
                            Message = ValidationMessages.Success,
                            Success = true,
                            StatusCode = (int)HttpStatusCode.OK,
                            Result = user,
                        };
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new DatabaseTransactionException(ex.Message);
                }
                return new ResponseOf<UpdateUserResult>
                {
                    Message = ValidationMessages.Failure,
                    StatusCode = (int)HttpStatusCode.ExpectationFailed,
                    Result = null!,
                };
            }
        }
    }
}
