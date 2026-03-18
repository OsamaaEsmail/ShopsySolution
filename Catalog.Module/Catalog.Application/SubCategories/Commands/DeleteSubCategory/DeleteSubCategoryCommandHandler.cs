using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.SubCategories.Commands.DeleteSubCategory;

public class DeleteSubCategoryCommandHandler(ISubCategoryService subCategoryService) : ICommandHandler<DeleteSubCategoryCommand>
{
    public async Task<Result> Handle(DeleteSubCategoryCommand request, CancellationToken ct)
    {
        return await subCategoryService.DeleteAsync(request.SubCategoryId, ct);
    }
}