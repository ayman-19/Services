﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Services.Domain.Entities;
using Services.Shared.Responses;

namespace Services.Application.Features.Services.Commands.Create
{
    public sealed record CreateServiceCommand(
        string name,
        string description,
        Guid categoryId,
        IFormFile File,
        Guid? DiscountId
    ) : IRequest<ResponseOf<CreateServiceResult>>
    {
        public static implicit operator Service(CreateServiceCommand serviceCommand) =>
            new()
            {
                Name = serviceCommand.name,
                Description = serviceCommand.description,
                CategoryId = serviceCommand.categoryId,
                DiscountId = serviceCommand.DiscountId,
            };
    }
}
