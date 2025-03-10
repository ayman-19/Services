using System.Net;
using MediatR;
using Services.Domain.Abstraction;
using Services.Shared.Exceptions;
using Services.Shared.Responses;
using Services.Shared.ValidationMessages;

namespace Services.Application.Features.Categories.Commands.Delete
{
    public sealed class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Response>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryHandler(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(
            DeleteCategoryCommand request,
            CancellationToken cancellationToken
        )
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    await _categoryRepository.DeleteByIdAsync(request.id, cancellationToken);
                    await transaction.CommitAsync();
                    return new()
                    {
                        Message = ValidationMessages.Success,
                        Success = true,
                        StatusCode = (int)HttpStatusCode.OK,
                    };
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw new DatabaseTransactionException(ValidationMessages.Database.Error);
                }
            }
        }
    }
}
