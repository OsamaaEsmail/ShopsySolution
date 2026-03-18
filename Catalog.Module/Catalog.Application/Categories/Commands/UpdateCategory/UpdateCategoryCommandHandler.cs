using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler(ICategoryService categoryService) : ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        return await categoryService.UpdateAsync(request.CategoryId, request.CategoryName, ct);
    }
}