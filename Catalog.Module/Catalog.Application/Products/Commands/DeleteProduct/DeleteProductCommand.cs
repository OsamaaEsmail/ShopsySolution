using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : ICommand;