using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.Domain.Abstraction;
using Services.Domain.Models;
using Services.Domain.Repositories;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.Login;

public sealed class LoginUserHandler(
    IJWTManager _jwtManager,
    IUserRepository _userRepository,
    IPasswordHasher<User> _passwordHasher,
    IJobs _jobs
) : IRequestHandler<LoginUserCommand, ResponseOf<LoginUserResult>>
{
    public async Task<ResponseOf<LoginUserResult>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var user =
                await _userRepository.GetByEmailAsync(request.email)
                ?? throw new InvalidException(ValidationMessages.Users.NotFound);

            if (!VerifyPassword(user, request.password))
                throw new InvalidException(ValidationMessages.Users.IncorrectPassword);

            if (!user.ConfirmAccount)
            {
                await HandleUnconfirmedAccount(user, cancellationToken);
                throw new NotConfirmEmail(ValidationMessages.Users.EmailNotConfirmed);
            }

            //await _userRepository.UpdateBranchAsync(user.Id, request.Latitude, request.Longitude);

            var result = await _jwtManager.LoginAsync(user);

            return new ResponseOf<LoginUserResult>
            {
                Message = ValidationMessages.Success,
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
                Result = result,
            };
        }
        catch (Exception ex)
        {
            throw new DatabaseTransactionException(ex.Message);
        }
    }

    private async Task HandleUnconfirmedAccount(User user, CancellationToken cancellationToken)
    {
        var code = await _jwtManager.GenerateCodeAsync();
        user.HashedCode(_passwordHasher, code);
        await _userRepository.UpdateCodeAsync(user.Id, user.Code!);
        await _jobs.SendEmailByJobAsync(user.Email, $"To Confirm Email Code: <h3>{code}</h3>");
    }

    private bool VerifyPassword(User user, string password) =>
        _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password)
        == PasswordVerificationResult.Success;
}
