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

public class StockServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<StockService>> _logger;

    public StockServiceTests()
    {
        var config = new TypeAdapterConfig();
        config.NewConfig<Stock, StockResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Size, src => src.Size)
            .Map(dest => dest.Color, src => src.Color)
            .Map(dest => dest.QuantityAvailable, src => src.QuantityAvailable)
            .Map(dest => dest.AddedDate, src => src.AddedDate)
            .Map(dest => dest.LastUpdated, src => src.LastUpdated);

        _mapper = new Mapper(config);
        _logger = new Mock<ILogger<StockService>>();
    }

    [Fact]
    public async Task AddAsync_WithValidProduct_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var product = new Product
        {
            ProductName = "iPhone",
            ProductDescription = "Phone",
            Price = 999,
            CategoryId = Guid.NewGuid(),
            SubCategoryId = Guid.NewGuid(),
            VendorId = Guid.NewGuid()
        };
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        var service = new StockService(context, _mapper, _logger.Object);

        // Act
        var result = await service.AddAsync(product.Id, "128GB", "Black", 50);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task AddAsync_WithInvalidProduct_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var service = new StockService(context, _mapper, _logger.Object);

        // Act
        var result = await service.AddAsync(Guid.NewGuid(), "128GB", "Black", 50);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.ProductNotFound);
    }

    [Fact]
    public async Task GetByProductAsync_WithValidProduct_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var product = new Product
        {
            ProductName = "iPhone",
            ProductDescription = "Phone",
            Price = 999,
            CategoryId = Guid.NewGuid(),
            SubCategoryId = Guid.NewGuid(),
            VendorId = Guid.NewGuid()
        };
        await context.Products.AddAsync(product);
        await context.Stocks.AddAsync(new Stock { ProductId = product.Id, Size = "128GB", Color = "Black", QuantityAvailable = 50 });
        await context.Stocks.AddAsync(new Stock { ProductId = product.Id, Size = "256GB", Color = "White", QuantityAvailable = 30 });
        await context.SaveChangesAsync();

        var service = new StockService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetByProductAsync(product.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Count().Should().Be(2);
    }

    [Fact]
    public async Task GetByProductAsync_WithInvalidProduct_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var service = new StockService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetByProductAsync(Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ProductErrors.ProductNotFound);
    }

    [Fact]
    public async Task UpdateAsync_WithExistingStock_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var stock = new Stock
        {
            ProductId = Guid.NewGuid(),
            Size = "128GB",
            Color = "Black",
            QuantityAvailable = 50
        };
        await context.Stocks.AddAsync(stock);
        await context.SaveChangesAsync();

        var service = new StockService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateAsync(stock.Id, "256GB", "White", 100);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var updated = await context.Stocks.FindAsync(stock.Id);
        updated!.Size.Should().Be("256GB");
        updated.Color.Should().Be("White");
        updated.QuantityAvailable.Should().Be(100);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistingStock_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var service = new StockService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateAsync(Guid.NewGuid(), "256GB", "White", 100);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(StockErrors.StockNotFound);
    }
}