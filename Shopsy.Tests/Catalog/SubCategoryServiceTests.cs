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

public class SubCategoryServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<SubCategoryService>> _logger;

    public SubCategoryServiceTests()
    {
        var config = new TypeAdapterConfig();
        config.NewConfig<SubCategory, SubCategoryResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.SubCategoryName, src => src.SubCategoryName)
            .Map(dest => dest.CategoryId, src => src.CategoryId);

        _mapper = new Mapper(config);
        _logger = new Mock<ILogger<SubCategoryService>>();
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var category = new Category { CategoryName = "Electronics" };
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        var service = new SubCategoryService(context, _mapper, _logger.Object);

        // Act
        var result = await service.CreateAsync("Phones", category.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateAsync_WithInvalidCategory_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var service = new SubCategoryService(context, _mapper, _logger.Object);

        // Act
        var result = await service.CreateAsync("Phones", Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.CategoryNotFound);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateName_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var category = new Category { CategoryName = "Electronics" };
        await context.Categories.AddAsync(category);
        await context.SubCategories.AddAsync(new SubCategory { SubCategoryName = "Phones", CategoryId = category.Id });
        await context.SaveChangesAsync();

        var service = new SubCategoryService(context, _mapper, _logger.Object);

        // Act
        var result = await service.CreateAsync("Phones", category.Id);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SubCategoryErrors.DuplicatedSubCategory);
    }

    [Fact]
    public async Task GetByCategoryAsync_WithValidCategory_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var category = new Category { CategoryName = "Electronics" };
        await context.Categories.AddAsync(category);
        await context.SubCategories.AddAsync(new SubCategory { SubCategoryName = "Phones", CategoryId = category.Id });
        await context.SubCategories.AddAsync(new SubCategory { SubCategoryName = "Laptops", CategoryId = category.Id });
        await context.SaveChangesAsync();

        var service = new SubCategoryService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetByCategoryAsync(category.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Count().Should().Be(2);
    }

    [Fact]
    public async Task GetByCategoryAsync_WithInvalidCategory_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var service = new SubCategoryService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetByCategoryAsync(Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.CategoryNotFound);
    }

    [Fact]
    public async Task UpdateAsync_WithExisting_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var category = new Category { CategoryName = "Electronics" };
        var sub = new SubCategory { SubCategoryName = "Phones", CategoryId = category.Id };
        await context.Categories.AddAsync(category);
        await context.SubCategories.AddAsync(sub);
        await context.SaveChangesAsync();

        var service = new SubCategoryService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateAsync(sub.Id, "Smartphones");

        // Assert
        result.IsSuccess.Should().BeTrue();

        var updated = await context.SubCategories.FindAsync(sub.Id);
        updated!.SubCategoryName.Should().Be("Smartphones");
    }

    [Fact]
    public async Task UpdateAsync_WithNonExisting_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var service = new SubCategoryService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateAsync(Guid.NewGuid(), "Smartphones");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SubCategoryErrors.SubCategoryNotFound);
    }

    [Fact]
    public async Task DeleteAsync_WithExisting_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var category = new Category { CategoryName = "Electronics" };
        var sub = new SubCategory { SubCategoryName = "Phones", CategoryId = category.Id };
        await context.Categories.AddAsync(category);
        await context.SubCategories.AddAsync(sub);
        await context.SaveChangesAsync();

        var service = new SubCategoryService(context, _mapper, _logger.Object);

        // Act
        var result = await service.DeleteAsync(sub.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var deleted = await context.SubCategories.FindAsync(sub.Id);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WithNonExisting_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var service = new SubCategoryService(context, _mapper, _logger.Object);

        // Act
        var result = await service.DeleteAsync(Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(SubCategoryErrors.SubCategoryNotFound);
    }
}