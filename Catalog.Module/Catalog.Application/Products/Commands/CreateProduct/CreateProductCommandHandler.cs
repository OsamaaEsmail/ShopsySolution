using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(IProductService productService) : ICommandHandler<CreateProductCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken ct)
    {
        return await productService.CreateAsync(
            request.ProductName,
            request.ProductDescription,
            request.Price,
            request.Currency,
            request.CategoryId,
            request.SubCategoryId,
            request.VendorId,
            ct);
    }
}