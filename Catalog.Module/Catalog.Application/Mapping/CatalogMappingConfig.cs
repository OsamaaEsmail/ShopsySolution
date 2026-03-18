using Catalog.Application.DtoContracts;
using Catalog.Domain.Entities;
using Mapster;

namespace Catalog.Application.Mapping;

public class CatalogMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, ProductResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.ProductName, src => src.ProductName)
            .Map(dest => dest.ProductDescription, src => src.ProductDescription)
            .Map(dest => dest.Price, src => src.Price)
            .Map(dest => dest.Currency, src => src.Currency)
            .Map(dest => dest.IsAvailable, src => src.IsAvailable)
            .Map(dest => dest.TotalStock, src => src.Stocks.Sum(s => s.QuantityAvailable))
            .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.CategoryName : string.Empty)
            .Map(dest => dest.SubCategoryName, src => src.SubCategory != null ? src.SubCategory.SubCategoryName : string.Empty)
            .Map(dest => dest.VendorName, src => src.Vendor != null ? src.Vendor.VendorName : string.Empty)
            .Map(dest => dest.ImageUrls, src => src.Images.Select(i => i.ImageUrl).ToList())
            .Map(dest => dest.CreatedOn, src => src.CreatedOn);

        config.NewConfig<Category, CategoryResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.CategoryName, src => src.CategoryName)
            .Map(dest => dest.SubCategories, src => src.SubCategories);

        config.NewConfig<SubCategory, SubCategoryResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.SubCategoryName, src => src.SubCategoryName)
            .Map(dest => dest.CategoryId, src => src.CategoryId);

        config.NewConfig<Vendor, VendorResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.VendorName, src => src.VendorName)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.VendorPicUrl, src => src.VendorPicUrl);

        config.NewConfig<Stock, StockResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Size, src => src.Size)
            .Map(dest => dest.Color, src => src.Color)
            .Map(dest => dest.QuantityAvailable, src => src.QuantityAvailable)
            .Map(dest => dest.AddedDate, src => src.AddedDate)
            .Map(dest => dest.LastUpdated, src => src.LastUpdated);
    }
}