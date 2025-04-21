using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.Domain.Abstraction;
using Services.Domain.Models;
using Services.Domain.Repositories;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.ResetPassword
{
    public sealed class ResetPasswordUserHandler
        : IRequestHandler<ResetPasswordUserCommand, Response>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;

        public ResetPasswordUserHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher<User> passwordHasher
        )
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<Response> Handle(
            ResetPasswordUserCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    User user = await _userRepository.GetByEmailAsync(request.email);
                    if (
                        _passwordHasher.VerifyHashedPassword(user, user.Code, request.code)
                        != PasswordVerificationResult.Success
                    )
                        throw new InvalidException(ValidationMessages.Users.VerifyCode);

                    user.HashPassword(_passwordHasher, request.password);
                    int modify = await _unitOfWork.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return new Response
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new DatabaseTransactionException(ex.Message);
                }
            }
        }
    }
}
