using Catalog.Application.DtoContracts;
using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Products.Queries.GetAllProducts;

public class GetAllProductsQueryHandler(IProductService productService) : IQueryHandler<GetAllProductsQuery, PaginatedList<ProductResponse>>
{
    public async Task<Result<PaginatedList<ProductResponse>>> Handle(GetAllProductsQuery request, CancellationToken ct)
    {
        return await productService.GetAllAsync(request.PageNumber, request.PageSize, request.CategoryId, ct);
    }
}