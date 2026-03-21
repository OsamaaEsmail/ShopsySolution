using Catalog.Application.DtoContracts;
using Catalog.Domain.Entities;
using Catalog.Domain.Errors;
using Catalog.Infrastructure.Services;
using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Shopsy.Tests.Helpers;

namespace Shopsy.Tests.Catalog;

public class ProductServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<ProductService>> _logger;

    public ProductServiceTests()
    {
        var config = new TypeAdapterConfig();
        config.NewConfig<Product, ProductResponse>()
            .Map(dest => dest.TotalStock, src => src.Stocks.Sum(s => s.QuantityAvailable))
            .Map(dest => dest.CategoryName, src => src.Category != null ? src.Category.CategoryName : string.Empty)
            .Map(dest => dest.SubCategoryName, src => src.SubCategory != null ? src.SubCategory.SubCategoryName : string.Empty)
            .Map(dest => dest.VendorName, src => src.Vendor != null ? src.Vendor.VendorName : string.Empty)
            .Map(dest => dest.ImageUrls, src => src.Images.Select(i => i.ImageUrl).ToList())
            .Map(dest => dest.CreatedOn, src => src.CreatedOn);

        _mapper = new Mapper(config);
        _logger = new Mock<ILogger<ProductService>>();
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var category = new Category { CategoryName = "Electronics" };
        var subCategory = new SubCategory { SubCategoryName = "Phones", CategoryId = category.Id };
        var vendor = new Vendor { VendorName = "Apple", Email = "apple@test.com", PhoneNumber = "123" };

        await context.Categories.AddAsync(category);
        await context.SubCategories.AddAsync(subCategory);
        await context.Vendors.AddAsync(vendor);
        await context.SaveChangesAsync();

        var service = new ProductService(context, _mapper, _logger.Object);

        // Act
        var result = await service.CreateAsync(
            "iPhone 15", "Latest iPhone", 999.99m, "USD",
            category.Id, subCategory.Id, vendor.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var product = await context.Products.FindAsync(result.Value);
        product.Should().NotBeNull();
        product!.ProductName.Should().Be("iPhone 15");
        product.Price.Should().Be(999.99m);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidCategory_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var service = new ProductService(context, _mapper, _logger.Object);

        // Act
        var result = await service.CreateAsync(
            "iPhone 15", "Latest iPhone", 999.99m, "USD",
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.CategoryNotFound);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingProduct_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var category = new Category { CategoryName = "Electronics" };
        var subCategory = new SubCategory { SubCategoryName = "Phones", CategoryId = category.Id };
        var vendor = new Vendor { VendorName = "Apple", Email = "apple@test.com", PhoneNumber = "123" };
        var product = new Product
        {
            ProductName = "iPhone 15",
            ProductDescription = "Latest iPhone",
            Price = 999.99m,
            CategoryId = category.Id,
            SubCategoryId = subCategory.Id,
            VendorId = vendor.Id
        };

        await context.Categories.AddAsync(category);
        await context.SubCategories.AddAsync(subCategory);
        await context.Vendors.AddAsync(vendor);
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        var service = new ProductService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetByIdAsync(product.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.ProductName.Should().Be("iPhone 15");
        result.Value.Price.Should().Be(999.99m);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingProduct_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var service = new ProductService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.ProductNotFound);
    }

    [Fact]
    public async Task UpdateAsync_WithExistingProduct_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var category = new Category { CategoryName = "Electronics" };
        var subCategory = new SubCategory { SubCategoryName = "Phones", CategoryId = category.Id };
        var vendor = new Vendor { VendorName = "Apple", Email = "apple@test.com", PhoneNumber = "123" };
        var product = new Product
        {
            ProductName = "iPhone 15",
            ProductDescription = "Latest iPhone",
            Price = 999.99m,
            CategoryId = category.Id,
            SubCategoryId = subCategory.Id,
            VendorId = vendor.Id
        };

        await context.Categories.AddAsync(category);
        await context.SubCategories.AddAsync(subCategory);
        await context.Vendors.AddAsync(vendor);
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        var service = new ProductService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateAsync(product.Id, "iPhone 16", "New iPhone", 1099.99m, "USD");

        // Assert
        result.IsSuccess.Should().BeTrue();

        var updated = await context.Products.FindAsync(product.Id);
        updated!.ProductName.Should().Be("iPhone 16");
        updated.Price.Should().Be(1099.99m);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistingProduct_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var service = new ProductService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateAsync(Guid.NewGuid(), "iPhone 16", "New iPhone", 1099.99m, "USD");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.ProductNotFound);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingProduct_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var product = new Product
        {
            ProductName = "iPhone 15",
            ProductDescription = "Latest iPhone",
            Price = 999.99m,
            CategoryId = Guid.NewGuid(),
            SubCategoryId = Guid.NewGuid(),
            VendorId = Guid.NewGuid()
        };

        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        var service = new ProductService(context, _mapper, _logger.Object);

        // Act
        var result = await service.DeleteAsync(product.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var deleted = await context.Products.FindAsync(product.Id);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingProduct_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var service = new ProductService(context, _mapper, _logger.Object);

        // Act
        var result = await service.DeleteAsync(Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.ProductNotFound);
    }
}