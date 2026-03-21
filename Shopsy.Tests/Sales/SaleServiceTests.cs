using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Sales.Application.DtoContracts;
using Sales.Domain.Entities;
using Sales.Domain.Enums;
using Sales.Domain.Errors;
using Sales.Infrastructure.Services;
using Shopsy.Tests.Helpers;

namespace Shopsy.Tests.Sales;

public class SaleServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<SaleService>> _logger;

    public SaleServiceTests()
    {
        var config = new TypeAdapterConfig();
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

        _mapper = new Mapper(config);
        _logger = new Mock<ILogger<SaleService>>();
    }

    private List<SaleItemResponse> CreateTestProducts()
    {
        return new List<SaleItemResponse>
        {
            new(Guid.Empty, Guid.NewGuid(), 799.99m),
            new(Guid.Empty, Guid.NewGuid(), 899.99m)
        };
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateSalesContext();
        var service = new SaleService(context, _mapper, _logger.Object);

        // Act
        var result = await service.CreateAsync(
            "Summer Sale", SaleType.Percentage, 20,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(30),
            null, CreateTestProducts());

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        var sale = await context.Sales.FindAsync(result.Value);
        sale.Should().NotBeNull();
        sale!.SaleName.Should().Be("Summer Sale");
        sale.DiscountPercentage.Should().Be(20);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidDateRange_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateSalesContext();
        var service = new SaleService(context, _mapper, _logger.Object);

        // Act
        var result = await service.CreateAsync(
            "Bad Sale", SaleType.Percentage, 20,
            DateTime.UtcNow.AddDays(30), DateTime.UtcNow,
            null, CreateTestProducts());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SaleErrors.InvalidDateRange);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateName_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateSalesContext();
        await context.Sales.AddAsync(new Sale
        {
            SaleName = "Summer Sale",
            SaleType = SaleType.Percentage,
            DiscountPercentage = 20,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30)
        });
        await context.SaveChangesAsync();

        var service = new SaleService(context, _mapper, _logger.Object);

        // Act
        var result = await service.CreateAsync(
            "Summer Sale", SaleType.Percentage, 30,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(15),
            null, CreateTestProducts());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SaleErrors.DuplicatedSale);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingSale_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateSalesContext();
        var sale = new Sale
        {
            SaleName = "Summer Sale",
            SaleType = SaleType.Percentage,
            DiscountPercentage = 20,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30)
        };
        await context.Sales.AddAsync(sale);
        await context.SaveChangesAsync();

        var service = new SaleService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetByIdAsync(sale.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.SaleName.Should().Be("Summer Sale");
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingSale_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateSalesContext();
        var service = new SaleService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SaleErrors.SaleNotFound);
    }

    [Fact]
    public async Task GetActiveSalesAsync_ReturnsOnlyActive()
    {
        // Arrange
        var context = TestDbContextFactory.CreateSalesContext();
        await context.Sales.AddAsync(new Sale
        {
            SaleName = "Active Sale",
            SaleType = SaleType.Percentage,
            DiscountPercentage = 20,
            StartDate = DateTime.UtcNow.AddDays(-5),
            EndDate = DateTime.UtcNow.AddDays(25)
        });
        await context.Sales.AddAsync(new Sale
        {
            SaleName = "Expired Sale",
            SaleType = SaleType.Percentage,
            DiscountPercentage = 10,
            StartDate = DateTime.UtcNow.AddDays(-30),
            EndDate = DateTime.UtcNow.AddDays(-1)
        });
        await context.Sales.AddAsync(new Sale
        {
            SaleName = "Future Sale",
            SaleType = SaleType.FixedAmount,
            DiscountPercentage = 50,
            StartDate = DateTime.UtcNow.AddDays(10),
            EndDate = DateTime.UtcNow.AddDays(40)
        });
        await context.SaveChangesAsync();

        var service = new SaleService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetActiveSalesAsync(1, 10);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().HaveCount(1);
        result.Value.Items.First().SaleName.Should().Be("Active Sale");
    }

    [Fact]
    public async Task UpdateAsync_WithExistingSale_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateSalesContext();
        var sale = new Sale
        {
            SaleName = "Summer Sale",
            SaleType = SaleType.Percentage,
            DiscountPercentage = 20,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30)
        };
        await context.Sales.AddAsync(sale);
        await context.SaveChangesAsync();

        var service = new SaleService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateAsync(sale.Id, "Winter Sale", 30,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(60), null);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var updated = await context.Sales.FindAsync(sale.Id);
        updated!.SaleName.Should().Be("Winter Sale");
        updated.DiscountPercentage.Should().Be(30);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistingSale_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateSalesContext();
        var service = new SaleService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateAsync(Guid.NewGuid(), "Test", 10,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(30), null);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SaleErrors.SaleNotFound);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidDateRange_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateSalesContext();
        var sale = new Sale
        {
            SaleName = "Summer Sale",
            SaleType = SaleType.Percentage,
            DiscountPercentage = 20,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30)
        };
        await context.Sales.AddAsync(sale);
        await context.SaveChangesAsync();

        var service = new SaleService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateAsync(sale.Id, "Test", 10,
            DateTime.UtcNow.AddDays(30), DateTime.UtcNow, null);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SaleErrors.InvalidDateRange);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingSale_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateSalesContext();
        var sale = new Sale
        {
            SaleName = "Summer Sale",
            SaleType = SaleType.Percentage,
            DiscountPercentage = 20,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30)
        };
        await context.Sales.AddAsync(sale);
        await context.SaveChangesAsync();

        var service = new SaleService(context, _mapper, _logger.Object);

        // Act
        var result = await service.DeleteAsync(sale.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var deleted = await context.Sales.FindAsync(sale.Id);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingSale_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateSalesContext();
        var service = new SaleService(context, _mapper, _logger.Object);

        // Act
        var result = await service.DeleteAsync(Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SaleErrors.SaleNotFound);
    }

    [Fact]
    public async Task UpdateAsync_WithDuplicateName_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateSalesContext();
        await context.Sales.AddAsync(new Sale
        {
            SaleName = "Summer Sale",
            SaleType = SaleType.Percentage,
            DiscountPercentage = 20,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30)
        });
        var sale2 = new Sale
        {
            SaleName = "Winter Sale",
            SaleType = SaleType.FixedAmount,
            DiscountPercentage = 10,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddDays(30)
        };
        await context.Sales.AddAsync(sale2);
        await context.SaveChangesAsync();

        var service = new SaleService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateAsync(sale2.Id, "Summer Sale", 10,
            DateTime.UtcNow, DateTime.UtcNow.AddDays(30), null);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SaleErrors.DuplicatedSale);
    }
}