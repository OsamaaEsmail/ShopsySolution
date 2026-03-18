using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Products.Commands.UpdateProduct;

public record UpdateProductCommand(
    Guid ProductId,
    string ProductName,
    string ProductDescription,
    decimal Price,
    string Currency) : ICommand;