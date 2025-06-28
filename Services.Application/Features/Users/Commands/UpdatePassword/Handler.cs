using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.Domain.Abstraction;
using Services.Domain.Models;
using Services.Domain.Repositories;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.UpdatePassword
{
    public sealed class RUpdatePasswordHandler(
        IUserRepository _userRepository,
        IUnitOfWork _unitOfWork,
        IPasswordHasher<User> _passwordHasher
    ) : IRequestHandler<UpdatePasswordCommand, Response>
    {
        public async Task<Response> Handle(
            UpdatePasswordCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    User user = await _userRepository.GetByIdAsync(request.Id);
                    if (
                        _passwordHasher.VerifyHashedPassword(
                            user,
                            user.HashedPassword,
                            request.oldPassword
                        ) != PasswordVerificationResult.Success
                    )
                        throw new InvalidException(ValidationMessages.Users.IncorrectPassword);

                    user.HashPassword(_passwordHasher, request.newPassword);
                    await _userRepository.UpdateAsync(user, cancellationToken);

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
