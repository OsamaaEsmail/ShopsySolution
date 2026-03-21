using Catalog.Application.DtoContracts;
using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler(ICategoryService categoryService) : IQueryHandler<GetAllCategoriesQuery, PaginatedList<CategoryResponse>>
{
    public async Task<Result<PaginatedList<CategoryResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken ct)
    {
        return await categoryService.GetAllAsync(request.PageNumber, request.PageSize, ct);
    }
}