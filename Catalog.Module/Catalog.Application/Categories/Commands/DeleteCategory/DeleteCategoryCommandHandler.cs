using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(ICategoryService categoryService) : ICommandHandler<DeleteCategoryCommand>
{
    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        return await categoryService.DeleteAsync(request.CategoryId, ct);
    }
}