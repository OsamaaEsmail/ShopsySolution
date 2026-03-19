using Cart.Application.DtoContracts;
using Shopsy.BuildingBlocks.CQRS;

namespace Cart.Application.Carts.Queries.GetCartByUser;

public record GetCartByUserQuery() : IQuery<CartResponse>;