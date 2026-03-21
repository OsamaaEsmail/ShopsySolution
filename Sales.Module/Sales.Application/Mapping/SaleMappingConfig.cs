using Mapster;
using Sales.Application.DtoContracts;
using Sales.Domain.Entities;

namespace Sales.Application.Mapping;

public class SaleMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Sale, SaleResponse>()
            .Map(dest => dest.SaleId, src => src.Id)
            .Map(dest => dest.SaleName, src => src.SaleName)
            .Map(dest => dest.SaleType, src => src.SaleType.ToString())
            .Map(dest => dest.DiscountPercentage, src => src.DiscountPercentage)
            .Map(dest => dest.StartDate, src => src.StartDate)
            .Map(dest => dest.EndDate, src => src.EndDate)
            .Map(dest => dest.IsActive, src => src.IsActive)
            .Map(dest => dest.SaleImage, src => src.SaleImage)
            .Map(dest => dest.Products, src => src.SaleItems);

        config.NewConfig<SaleItem, SaleItemResponse>()
            .Map(dest => dest.SaleItemId, src => src.Id)
            .Map(dest => dest.ProductId, src => src.ProductId)
            .Map(dest => dest.DiscountedPrice, src => src.DiscountedPrice);
    }
}