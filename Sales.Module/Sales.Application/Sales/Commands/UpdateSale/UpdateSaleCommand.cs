using Shopsy.BuildingBlocks.CQRS;

namespace Sales.Application.Sales.Commands.UpdateSale;

public record UpdateSaleCommand(
    Guid SaleId,
    string SaleName,
    decimal DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate,
    string? SaleImage) : ICommand;