using Order.Application.DtoContracts;
using Shopsy.BuildingBlocks.CQRS;

namespace Order.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    List<OrderItemResponse> Items,
    AddressResponse BillingAddress,
    AddressResponse ShippingAddress) : ICommand<Guid>;