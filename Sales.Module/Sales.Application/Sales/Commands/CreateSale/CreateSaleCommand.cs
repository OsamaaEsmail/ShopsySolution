using Sales.Application.DtoContracts;
using Sales.Domain.Enums;
using Shopsy.BuildingBlocks.CQRS;

namespace Sales.Application.Sales.Commands.CreateSale;

public record CreateSaleCommand(
    string SaleName,
    SaleType SaleType,
    decimal DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate,
    string? SaleImage,
    List<SaleItemResponse> Products) : ICommand<Guid>;