



using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopsy.BuildingBlocks.Abstractions;
using System.Security.Claims;
using User.Application.DtoContracts;
using User.Application.Interfaces;
using User.Domain.Consts;
using User.Domain.Entities;
using User.Domain.Errors;

namespace User.Infrastructure.Services;


public class RoleService(RoleManager<ApplicationRole> roleManager) : IRoleService
{
    public async Task<Result<IEnumerable<RoleResponse>>> GetAllAsync()
    {
        var roles = await roleManager.Roles
            .Select(r => new RoleResponse(r.Id, r.Name!, r.IsDeleted))
            .ToListAsync();

        return Result.Success<IEnumerable<RoleResponse>>(roles);
    }

    public async Task<Result<RoleDetailResponse>> GetByIdAsync(string roleId)
    {
        var role = await roleManager.FindByIdAsync(roleId);

        if (role is null)
            return Result.Failure<RoleDetailResponse>(RoleErrors.RoleNotFound);

        var claims = await roleManager.GetClaimsAsync(role);
        var permissions = claims
            .Where(c => c.Type == Permissions.Type)
            .Select(c => c.Value);

        return Result.Success(new RoleDetailResponse(
            role.Id, role.Name!, role.IsDeleted, permissions));
    }

    public async Task<Result> CreateAsync(string name, IEnumerable<string> permissions)
    {
        var roleExists = await roleManager.RoleExistsAsync(name);

        if (roleExists)
            return Result.Failure(RoleErrors.DuplicatedRole);

        var role = new ApplicationRole { Name = name };

        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        foreach (var permission in permissions)
            await roleManager.AddClaimAsync(role, new Claim(Permissions.Type, permission));

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(string roleId, string name, IEnumerable<string> permissions)
    {
        var role = await roleManager.FindByIdAsync(roleId);

        if (role is null)
            return Result.Failure(RoleErrors.RoleNotFound);

        role.Name = name;
        await roleManager.UpdateAsync(role);

        // Remove old permissions
        var existingClaims = await roleManager.GetClaimsAsync(role);
        var permissionClaims = existingClaims.Where(c => c.Type == Permissions.Type);

        foreach (var claim in permissionClaims)
            await roleManager.RemoveClaimAsync(role, claim);

        // Add new permissions
        foreach (var permission in permissions)
            await roleManager.AddClaimAsync(role, new Claim(Permissions.Type, permission));

        return Result.Success();
    }
}