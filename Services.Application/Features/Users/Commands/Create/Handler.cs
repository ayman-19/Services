using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Domain.Enums;
using Services.Domain.Models;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Users.Commands.Create;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response>
{
    private readonly IJWTManager _jwtManager;
    private readonly IWorkerRepository _workerRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IBranchRepository _branchRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJobs _jobs;
    private readonly IPasswordHasher<User> _passwordHasher;

    public CreateUserCommandHandler(
        IJWTManager jwtManager,
        IUnitOfWork unitOfWork,
        IPasswordHasher<User> passwordHasher,
        IJobs jobs,
        IWorkerRepository workerRepository,
        ICustomerRepository customerRepository,
        IBranchRepository branchRepository
    )
    {
        _jwtManager = jwtManager;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jobs = jobs;
        _workerRepository = workerRepository;
        _customerRepository = customerRepository;
        _branchRepository = branchRepository;
    }

    public async Task<Response> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var verificationCode = await _jwtManager.GenerateCodeAsync();

        switch (request.UserType)
        {
            case UserType.Worker:
                await CreateWorkerAsync(request, verificationCode, cancellationToken);
                break;

            case UserType.Customer:
                await CreateCustomerAsync(request, verificationCode, cancellationToken);
                break;

            case UserType.Agent:
                throw new NotSupportedException("Agent user type is not yet supported.");

            default:
                throw new ArgumentOutOfRangeException(nameof(request.UserType));
        }

        var changes = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (changes > 0)
        {
            await transaction.CommitAsync(cancellationToken);

            await _jobs.SendEmailByJobAsync(
                request.email,
                $"To Confirm Email Code: <h3>{verificationCode}</h3>"
            );

            return new Response
            {
                Message = ValidationMessages.Success,
                Success = true,
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        await transaction.RollbackAsync();
        throw new DatabaseTransactionException(ValidationMessages.Database.Error);
    }

    private async Task CreateWorkerAsync(
        CreateUserCommand request,
        string code,
        CancellationToken cancellationToken
    )
    {
        var worker = (Worker)request;

        HashCredentials(worker.User, request.password, code);

        worker.User.Branch = CreateBranch(request.Latitude, request.Longitude);

        worker.WorkerServices.Add(
            new WorkerService
            {
                Availabilty = false,
                Price = request.Price ?? 0,
                ServiceId = request.ServiceId ?? Guid.Empty,
            }
        );

        await _workerRepository.CreateAsync(worker, cancellationToken);
        worker.User.Token = await GetToken(worker.User);
    }

    private async Task CreateCustomerAsync(
        CreateUserCommand request,
        string code,
        CancellationToken cancellationToken
    )
    {
        var customer = (Customer)request;

        HashCredentials(customer.User, request.password, code);

        customer.User.Branch = CreateBranch(request.Latitude, request.Longitude);

        await _customerRepository.CreateAsync(customer, cancellationToken);
        customer.User.Token = await GetToken(customer.User);
    }

    private static Branch CreateBranch(double? lat, double? lng) =>
        new() { Latitude = lng ?? 0, Langitude = lat ?? 0 };

    private void HashCredentials(User user, string password, string code)
    {
        user.HashPassword(_passwordHasher, password);
        user.HashedCode(_passwordHasher, code);
    }

    private async Task<Token> GetToken(User user) => await _jwtManager.GenerateTokenAsync(user);
}
