using System.Net;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.Application.Features.Users.Commands.Create;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Domain.Enums;
using Services.Domain.Models;
using Services.Domain.Repositories;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response>
{
    private readonly IJWTManager _jwtManager;
    private readonly IWorkerRepository _workerRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJobs _jobs;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IRoleRepository _roleRepository;

    public CreateUserCommandHandler(
        IJWTManager jwtManager,
        IUnitOfWork unitOfWork,
        IPasswordHasher<User> passwordHasher,
        IJobs jobs,
        IWorkerRepository workerRepository,
        ICustomerRepository customerRepository,
        IRoleRepository roleRepository
    )
    {
        _jwtManager = jwtManager;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jobs = jobs;
        _workerRepository = workerRepository;
        _customerRepository = customerRepository;
        _roleRepository = roleRepository;
    }

    public async Task<Response> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        var verificationCode = await _jwtManager.GenerateCodeAsync();

        try
        {
            switch (request.UserType)
            {
                case UserType.Worker:
                    await HandleWorkerCreationAsync(request, verificationCode, cancellationToken);
                    break;

                case UserType.Customer:
                    await HandleCustomerCreationAsync(request, verificationCode, cancellationToken);
                    break;

                case UserType.Admin:
                    throw new NotSupportedException("Admin user type is not yet supported.");

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(request.UserType),
                        "Invalid user type."
                    );
            }

            var saved = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (saved > 0)
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
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception(ex.Message, ex);
        }
    }

    private async Task HandleWorkerCreationAsync(
        CreateUserCommand request,
        string code,
        CancellationToken cancellationToken
    )
    {
        var worker = (Worker)request;
        InitializeUser(worker.User, request.password, code, request.Latitude, request.Longitude);
        worker.WorkerServices.Add(
            new WorkerService
            {
                Availabilty = false,
                Price = request.Price ?? 0,
                ServiceId = request.ServiceId ?? Guid.Empty,
            }
        );

        await AssignRoleAsync(worker.User, UserType.Worker);
        worker.User.Token = await _jwtManager.GenerateTokenAsync(worker.User);
        await _workerRepository.CreateAsync(worker, cancellationToken);
    }

    private async Task HandleCustomerCreationAsync(
        CreateUserCommand request,
        string code,
        CancellationToken cancellationToken
    )
    {
        var customer = (Customer)request;
        InitializeUser(customer.User, request.password, code, request.Latitude, request.Longitude);
        await AssignRoleAsync(customer.User, UserType.Customer);
        customer.User.Token = await _jwtManager.GenerateTokenAsync(customer.User);
        await _customerRepository.CreateAsync(customer, cancellationToken);
    }

    private void InitializeUser(
        User user,
        string password,
        string code,
        double? latitude,
        double? longitude
    )
    {
        user.HashPassword(_passwordHasher, password);
        user.HashedCode(_passwordHasher, code);
        user.Branch = new Branch { Latitude = longitude ?? 0, Langitude = latitude ?? 0 };
    }

    private async Task AssignRoleAsync(User user, UserType userType)
    {
        var roleId = await _roleRepository.GetRoleIdByNameAsync(userType.ToString());
        user.UserRoles.Add(new UserRole { RoleId = roleId });
    }
}
