using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler(IProductService productService) : ICommandHandler<UpdateProductCommand>
{
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        return await productService.UpdateAsync(
            request.ProductId,
            request.ProductName,
            request.ProductDescription,
            request.Price,
            request.Currency,
            ct);
    }
}