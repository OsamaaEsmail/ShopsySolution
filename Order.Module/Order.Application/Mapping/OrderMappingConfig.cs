using Mapster;
using Order.Application.DtoContracts;
using Order.Domain.Entities;
using OrderEntity = Order.Domain.Entities.Order;

namespace Order.Application.Mapping;

public class OrderMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<OrderEntity, OrderResponse>()
            .Map(dest => dest.OrderId, src => src.Id)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.OrderDate, src => src.OrderDate)
            .Map(dest => dest.OrderStatus, src => src.OrderStatus.ToString())
            .Map(dest => dest.ShippingStatus, src => src.ShippingStatus)
            .Map(dest => dest.TotalQuantity, src => src.TotalQuantity)
            .Map(dest => dest.TotalAmount, src => src.TotalAmount)
            .Map(dest => dest.Items, src => src.OrderItems)
            .Map(dest => dest.BillingAddress, src => src.BillingAddress)
            .Map(dest => dest.ShippingAddress, src => src.ShippingAddress)
            .Map(dest => dest.Payment, src => src.Payment);

        config.NewConfig<OrderItem, OrderItemResponse>()
            .Map(dest => dest.OrderItemId, src => src.Id)
            .Map(dest => dest.ProductId, src => src.ProductId)
            .Map(dest => dest.Size, src => src.Size)
            .Map(dest => dest.Color, src => src.Color)
            .Map(dest => dest.Quantity, src => src.Quantity)
            .Map(dest => dest.UnitPrice, src => src.UnitPrice)
            .Map(dest => dest.TotalPrice, src => src.TotalPrice);

        config.NewConfig<BillingAddress, AddressResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.FullName, src => src.FullName)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.EmailAddress, src => src.EmailAddress)
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.Country, src => src.Country)
            .Map(dest => dest.StateOrProvince, src => src.StateOrProvince)
            .Map(dest => dest.City, src => src.City)
            .Map(dest => dest.PostalOrZipCode, src => src.PostalOrZipCode);

        config.NewConfig<ShippingAddress, AddressResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.FullName, src => src.FullName)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.EmailAddress, src => src.EmailAddress)
            .Map(dest => dest.Address, src => src.Address)
            .Map(dest => dest.Country, src => src.Country)
            .Map(dest => dest.StateOrProvince, src => src.StateOrProvince)
            .Map(dest => dest.City, src => src.City)
            .Map(dest => dest.PostalOrZipCode, src => src.PostalOrZipCode);

        config.NewConfig<Payment, PaymentResponse>()
            .Map(dest => dest.PaymentId, src => src.Id)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.PaymentDate, src => src.PaymentDate)
            .Map(dest => dest.PaymentMethod, src => src.PaymentMethod)
            .Map(dest => dest.PaymentStatus, src => src.PaymentStatus);
    }
}