using Order.Application.DtoContracts;
using Shopsy.BuildingBlocks.CQRS;

namespace Order.Application.Orders.Queries.GetMyOrders;

public record GetMyOrdersQuery() : IQuery<IEnumerable<OrderResponse>>;