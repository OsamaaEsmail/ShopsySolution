using Order.Application.DtoContracts;
using Shopsy.BuildingBlocks.CQRS;

namespace Order.Application.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId) : IQuery<OrderResponse>;