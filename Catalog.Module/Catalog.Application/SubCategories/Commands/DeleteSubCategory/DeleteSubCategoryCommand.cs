using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.SubCategories.Commands.DeleteSubCategory;

public record DeleteSubCategoryCommand(Guid SubCategoryId) : ICommand;