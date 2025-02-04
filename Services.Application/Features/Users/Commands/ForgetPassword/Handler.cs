using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Services.Application.Features.Users.Commands.Create;
using Services.Domain.Abstraction;
using Services.Domain.Models;
using Services.Domain.Repositories;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.ForgetPassword
{
    public sealed class ForgetPasswordUserHandler
        : IRequestHandler<ForgetPasswordUserCommand, ResponseOf<ForgetPasswordUserResult>>
    {
        private readonly IJWTManager _jwtManager;
        private readonly IEmailSender _emailSender;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;

        public ForgetPasswordUserHandler(
            IJWTManager jwtManager,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IEmailSender emailSender,
            IPasswordHasher<User> passwordHasher
        )
        {
            _jwtManager = jwtManager;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _passwordHasher = passwordHasher;
        }

        public async Task<ResponseOf<ForgetPasswordUserResult>> Handle(
            ForgetPasswordUserCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    User user = await _userRepository.GetByEmailAsync(request.email);
                    string code = await _jwtManager.GenerateCodeAsync();
                    user.HashedCode(_passwordHasher, code);
                    //await _userRepository.UpdateAsync(user, cancellationToken);
                    int modify = await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    if (modify > 0)
                        await _emailSender.SendEmailAsync(
                            user.Email,
                            ValidationMessages.User.ResetPassword,
                            $"To Confirm Email Code: <h3>{code}</h3>"
                        );

                    return new ResponseOf<ForgetPasswordUserResult>
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = user,
                    };
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw new DatabaseTransactionException(ValidationMessages.Database.Error);
                }
            }
        }
    }
}
