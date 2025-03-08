using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Context;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.Logout
{
    public sealed class LogoutUserHandler : IRequestHandler<LogoutUserCommand, Response>
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;

        public LogoutUserHandler(
            ITokenRepository tokenRepository,
            IUnitOfWork unitOfWork,
            IUserContext userContext
        )
        {
            _tokenRepository = tokenRepository;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task<Response> Handle(
            LogoutUserCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    (string, bool) userId = _userContext.UserId;

                    if (!userId.Item2)
                        throw new InvalidException(ValidationMessages.User.LogoutError);

                    await _tokenRepository.DeleteByUserIdAsync(Guid.Parse(userId.Item1));
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
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
