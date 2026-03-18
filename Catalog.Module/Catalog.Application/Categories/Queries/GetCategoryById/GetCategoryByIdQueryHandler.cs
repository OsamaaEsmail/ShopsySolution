using Catalog.Application.DtoContracts;
using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler(ICategoryService categoryService) : IQueryHandler<GetCategoryByIdQuery, CategoryResponse>
{
    public async Task<Result<CategoryResponse>> Handle(GetCategoryByIdQuery request, CancellationToken ct)
    {
        return await categoryService.GetByIdAsync(request.CategoryId, ct);
    }
}