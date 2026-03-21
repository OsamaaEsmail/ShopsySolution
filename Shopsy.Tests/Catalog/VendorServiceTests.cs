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

public class VendorServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<VendorService>> _logger;

    public VendorServiceTests()
    {
        var config = new TypeAdapterConfig();
        config.NewConfig<Vendor, VendorResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.VendorName, src => src.VendorName)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.VendorPicUrl, src => src.VendorPicUrl);

        _mapper = new Mapper(config);
        _logger = new Mock<ILogger<VendorService>>();
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var service = new VendorService(context, _mapper, _logger.Object);

        // Act
        var result = await service.CreateAsync("Apple Store", "apple@store.com", "+123456", null);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateEmail_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        await context.Vendors.AddAsync(new Vendor
        {
            VendorName = "Apple Store",
            Email = "apple@store.com",
            PhoneNumber = "+123456"
        });
        await context.SaveChangesAsync();

        var service = new VendorService(context, _mapper, _logger.Object);

        // Act
        var result = await service.CreateAsync("Apple Store 2", "apple@store.com", "+789", null);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(VendorErrors.DuplicatedVendor);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingVendor_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var vendor = new Vendor { VendorName = "Apple", Email = "apple@test.com", PhoneNumber = "123" };
        await context.Vendors.AddAsync(vendor);
        await context.SaveChangesAsync();

        var service = new VendorService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetByIdAsync(vendor.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.VendorName.Should().Be("Apple");
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingVendor_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var service = new VendorService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(VendorErrors.VendorNotFound);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingVendor_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var vendor = new Vendor { VendorName = "Apple", Email = "apple@test.com", PhoneNumber = "123" };
        await context.Vendors.AddAsync(vendor);
        await context.SaveChangesAsync();

        var service = new VendorService(context, _mapper, _logger.Object);

        // Act
        var result = await service.DeleteAsync(vendor.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var deleted = await context.Vendors.FindAsync(vendor.Id);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_WithDuplicateEmail_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        await context.Vendors.AddAsync(new Vendor { VendorName = "Apple", Email = "apple@test.com", PhoneNumber = "123" });
        var vendor2 = new Vendor { VendorName = "Samsung", Email = "samsung@test.com", PhoneNumber = "456" };
        await context.Vendors.AddAsync(vendor2);
        await context.SaveChangesAsync();

        var service = new VendorService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateAsync(vendor2.Id, "Samsung", "apple@test.com", "456", null);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(VendorErrors.DuplicatedVendor);
    }
}