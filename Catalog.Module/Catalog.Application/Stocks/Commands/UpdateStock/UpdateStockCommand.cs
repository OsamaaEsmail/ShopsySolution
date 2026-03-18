using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.Stocks.Commands.UpdateStock;

public record UpdateStockCommand(
    Guid StockId,
    string? Size,
    string? Color,
    int Quantity) : ICommand;