using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string ProductName,
    string ProductDescription,
    decimal Price,
    string Currency,
    Guid CategoryId,
    Guid SubCategoryId,
    Guid VendorId) : ICommand<Guid>;