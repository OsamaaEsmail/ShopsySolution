using Catalog.Application.Interfaces;
using Shopsy.BuildingBlocks.Abstractions;
using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler(IProductService productService) : ICommandHandler<DeleteProductCommand>
{
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken ct)
    {
        return await productService.DeleteAsync(request.ProductId, ct);
    }
}