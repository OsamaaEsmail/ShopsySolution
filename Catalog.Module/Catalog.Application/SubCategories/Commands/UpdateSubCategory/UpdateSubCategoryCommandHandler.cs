using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.SubCategories.Commands.UpdateSubCategory;

public class UpdateSubCategoryCommandHandler(ISubCategoryService subCategoryService) : ICommandHandler<UpdateSubCategoryCommand>
{
    public async Task<Result> Handle(UpdateSubCategoryCommand request, CancellationToken ct)
    {
        return await subCategoryService.UpdateAsync(request.SubCategoryId, request.SubCategoryName, ct);
    }
}