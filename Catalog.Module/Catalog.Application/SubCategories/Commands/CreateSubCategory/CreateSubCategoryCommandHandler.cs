using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.SubCategories.Commands.CreateSubCategory;

public class CreateSubCategoryCommandHandler(ISubCategoryService subCategoryService) : ICommandHandler<CreateSubCategoryCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateSubCategoryCommand request, CancellationToken ct)
    {
        return await subCategoryService.CreateAsync(request.SubCategoryName, request.CategoryId, ct);
    }
}