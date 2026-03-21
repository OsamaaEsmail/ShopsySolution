using Shopsy.BuildingBlocks.CQRS;

namespace Sales.Application.Sales.Commands.DeleteSale;

public record DeleteSaleCommand(Guid SaleId) : ICommand;