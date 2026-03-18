using Shopsy.BuildingBlocks.CQRS;

namespace Catalog.Application.SubCategories.Commands.CreateSubCategory;

public record CreateSubCategoryCommand(string SubCategoryName, Guid CategoryId) : ICommand<Guid>;