using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Services.Domain.Abstraction;
using Services.Domain.Models;
using Services.Domain.Repositories;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.Delete
{
    public sealed class DeleteUserHandler(
        IUserRepository _userRepository,
        IBookingRepository _bookingRepository,
        IUnitOfWork _unitOfWork
    ) : IRequestHandler<DeleteUserCommand, Response>
    {
        public async Task<Response> Handle(
            DeleteUserCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken))
            {
                User user = await _userRepository.GetByIdAsync(request.userId);
                await _bookingRepository.DeleteByUserIdAsync(
                    user.Id,
                    user.UserType,
                    cancellationToken
                );
                EntityEntry<User> userEntry = await _userRepository.DeleteAsync(
                    user,
                    cancellationToken
                );
                var success = await _unitOfWork.SaveChangesAsync(cancellationToken);
                if (success > 0)
                {
                    await transaction.CommitAsync(cancellationToken);
                    return new Response
                    {
                        Success = true,
                        Message = ValidationMessages.Success,
                        StatusCode = (int)HttpStatusCode.OK,
                    };
                }
                await transaction.RollbackAsync();
                throw new DatabaseTransactionException(ValidationMessages.Database.Error);
            }
        }
    }
}
