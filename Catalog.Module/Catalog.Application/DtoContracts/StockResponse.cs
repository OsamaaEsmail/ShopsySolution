namespace Catalog.Application.DtoContracts;

public record StockResponse(
    Guid Id,
    string? Size,
    string? Color,
    int QuantityAvailable,
    DateTime AddedDate,
    DateTime LastUpdated);