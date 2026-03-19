using Asp.Versioning;
using Catalog.Application.SubCategories.Commands.CreateSubCategory;
using Catalog.Application.SubCategories.Commands.DeleteSubCategory;
using Catalog.Application.SubCategories.Commands.UpdateSubCategory;
using Catalog.Application.SubCategories.Queries.GetAllSubCategories;
using Catalog.Application.SubCategories.Queries.GetSubCategoriesByCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopsy.BuildingBlocks.Abstractions;
using User.Domain.Consts;
using User.Infrastructure.Authentication.Filters;

namespace Shopsy.API.Controllers.Catalog;

[ApiVersion(1)]
[Route("api/[controller]")]
[ApiController]
public class SubCategoriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.GetSubCategories)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAllSubCategoriesQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("by-category/{categoryId:guid}")]
    [HasPermission(Permissions.GetSubCategories)]
    public async Task<IActionResult> GetByCategory(Guid categoryId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSubCategoriesByCategoryQuery(categoryId), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    [HasPermission(Permissions.AddSubCategories)]
    public async Task<IActionResult> Create([FromBody] CreateSubCategoryCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("{id:guid}")]
    [HasPermission(Permissions.UpdateSubCategories)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSubCategoryRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateSubCategoryCommand(id, request.SubCategoryName), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.DeleteSubCategories)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteSubCategoryCommand(id), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}

public record UpdateSubCategoryRequest(string SubCategoryName);