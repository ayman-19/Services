using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.Domain.Abstraction;
using Services.Domain.Models;
using Services.Domain.Repositories;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.Confirm
{
    public sealed class ConfirmUserHandler : IRequestHandler<ConfirmUserCommand, Response>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;

        public ConfirmUserHandler(
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
            ConfirmUserCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    User user = await _userRepository.GetByEmailAsync(request.email);
                    if (
                        _passwordHasher.VerifyHashedPassword(user, user.Code, request.code)
                        != PasswordVerificationResult.Success
                    )
                        throw new InvalidException(ValidationMessages.User.VerifyCode);

                    user.ConfirmAccount = true;
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return new Response
                    {
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Message = ValidationMessages.Success,
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
