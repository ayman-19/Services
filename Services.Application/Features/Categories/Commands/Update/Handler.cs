using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Services.Application.Features.Branchs.Comands.Update;
using Services.Domain.Abstraction;
using Services.Domain.Entities;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Categories.Commands.Update
{
    public sealed class UpdateCategoryHandler
        : IRequestHandler<UpdateCategoryCommand, ResponseOf<UpdateCategoryResult>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseOf<UpdateCategoryResult>> Handle(
            UpdateCategoryCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    Category category = await _categoryRepository.GetByIdAsync(
                        request.Id,
                        cancellationToken
                    );
                    category.UpdateCategory(
                        request.Name,
                        request.ParentId == Guid.Empty ? null : request.ParentId
                    );
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return new()
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                        Result = category,
                    };
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw new DatabaseTransactionException(ValidationMessages.Database.Error);
                }
            }
        }
    }
}
