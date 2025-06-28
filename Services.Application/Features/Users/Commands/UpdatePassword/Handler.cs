using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.Application.Features.Users.Commands.ResetPassword;
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
