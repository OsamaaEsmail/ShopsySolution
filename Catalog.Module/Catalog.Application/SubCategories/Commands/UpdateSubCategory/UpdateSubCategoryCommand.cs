using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.SubCategories.Commands.UpdateSubCategory;

public record UpdateSubCategoryCommand(Guid SubCategoryId, string SubCategoryName) : ICommand;