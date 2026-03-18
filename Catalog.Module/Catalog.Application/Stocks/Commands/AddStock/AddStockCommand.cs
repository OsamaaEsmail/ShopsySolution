using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Stocks.Commands.AddStock;

public record AddStockCommand(
    Guid ProductId,
    string? Size,
    string? Color,
    int Quantity) : ICommand<Guid>;