using Asp.Versioning;
using Catalog.Application.Categories.Commands.CreateCategory;
using Catalog.Application.Categories.Commands.DeleteCategory;
using Catalog.Application.Categories.Commands.UpdateCategory;
using Catalog.Application.Categories.Queries.GetAllCategories;
using Catalog.Application.Categories.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shopsy.BuildingBlocks.Abstractions;
using User.Domain.Consts;
using User.Infrastructure.Authentication.Filters;

namespace Shopsy.API.Controllers.Catalog;

[ApiVersion(1)]
[Route("api/[controller]")]
[ApiController]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [HasPermission(Permissions.GetCategories)]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllCategoriesQuery(pageNumber, pageSize), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{id:guid}")]
    [HasPermission(Permissions.GetCategories)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetCategoryByIdQuery(id), ct);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    [HasPermission(Permissions.AddCategories)]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value) : result.ToProblem();
    }

    [HttpPut("{id:guid}")]
    [HasPermission(Permissions.UpdateCategories)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken ct)
    {
        var result = await mediator.Send(new UpdateCategoryCommand(id, request.CategoryName), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id:guid}")]
    [HasPermission(Permissions.DeleteCategories)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteCategoryCommand(id), ct);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}

public record UpdateCategoryRequest(string CategoryName);