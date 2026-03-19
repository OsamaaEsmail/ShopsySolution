using Cart.Application.DtoContracts;
using Cart.Domain.Entities;
using Mapster;

namespace Cart.Application.Mapping;

public class CartMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Basket, CartResponse>()
            .Map(dest => dest.CartId, src => src.Id)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.TotalQuantity, src => src.TotalQuantity)
            .Map(dest => dest.TotalAmount, src => src.TotalAmount)
            .Map(dest => dest.Items, src => src.Items);

        config.NewConfig<BasketItem, CartItemResponse>()
            .Map(dest => dest.CartItemId, src => src.Id)
            .Map(dest => dest.ProductId, src => src.ProductId)
            .Map(dest => dest.Color, src => src.Color)
            .Map(dest => dest.Size, src => src.Size)
            .Map(dest => dest.UnitPrice, src => src.UnitPrice)
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.TotalPrice, src => src.TotalPrice);
    }
}