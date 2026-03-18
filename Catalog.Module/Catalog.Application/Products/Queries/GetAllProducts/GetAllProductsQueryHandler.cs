using Catalog.Application.DtoContracts;
using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Products.Queries.GetAllProducts;

public class GetAllProductsQueryHandler(IProductService productService) : IQueryHandler<GetAllProductsQuery, IEnumerable<ProductResponse>>
{
    public async Task<Result<IEnumerable<ProductResponse>>> Handle(GetAllProductsQuery request, CancellationToken ct)
    {
        return await productService.GetAllAsync(request.CategoryId, ct);
    }
}