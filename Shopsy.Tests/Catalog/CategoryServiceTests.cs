using Catalog.Application.DtoContracts;
using Catalog.Domain.Entities;
using Catalog.Domain.Errors;
using Catalog.Infrastructure.Services;
using FluentAssertions;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Moq;
using Shopsy.Tests.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Shopsy.Tests.Catalog;

public class CategoryServiceTests
{
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<CategoryService>> _logger;

    public CategoryServiceTests()
    {
        var config = new TypeAdapterConfig();
        config.NewConfig<Category, CategoryResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.CategoryName, src => src.CategoryName)
            .Map(dest => dest.SubCategories, src => src.SubCategories);

        config.NewConfig<SubCategory, SubCategoryResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.SubCategoryName, src => src.SubCategoryName)
            .Map(dest => dest.CategoryId, src => src.CategoryId);

        _mapper = new Mapper(config);
        _logger = new Mock<ILogger<CategoryService>>();
    }

    [Fact]
    public async Task CreateAsync_WithValidName_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var service = new CategoryService(context, _mapper, _logger.Object);

        // Act
        var result = await service.CreateAsync("Electronics");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateName_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        await context.Categories.AddAsync(new Category { CategoryName = "Electronics" });
        await context.SaveChangesAsync();

        var service = new CategoryService(context, _mapper, _logger.Object);

        // Act
        var result = await service.CreateAsync("Electronics");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.DuplicatedCategory);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingCategory_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var category = new Category { CategoryName = "Electronics" };
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        var service = new CategoryService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetByIdAsync(category.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.CategoryName.Should().Be("Electronics");
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingCategory_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var service = new CategoryService(context, _mapper, _logger.Object);

        // Act
        var result = await service.GetByIdAsync(Guid.NewGuid());

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.CategoryNotFound);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingCategory_ReturnsSuccess()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        var category = new Category { CategoryName = "Electronics" };
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        var service = new CategoryService(context, _mapper, _logger.Object);

        // Act
        var result = await service.DeleteAsync(category.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var deleted = await context.Categories.FindAsync(category.Id);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_WithDuplicateName_ReturnsFailure()
    {
        // Arrange
        var context = TestDbContextFactory.CreateCatalogContext();
        await context.Categories.AddAsync(new Category { CategoryName = "Electronics" });
        var category2 = new Category { CategoryName = "Clothing" };
        await context.Categories.AddAsync(category2);
        await context.SaveChangesAsync();

        var service = new CategoryService(context, _mapper, _logger.Object);

        // Act
        var result = await service.UpdateAsync(category2.Id, "Electronics");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CategoryErrors.DuplicatedCategory);
    }
}
