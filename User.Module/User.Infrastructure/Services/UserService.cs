using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopsy.BuildingBlocks.Abstractions;
using User.Application.DtoContracts;
using User.Application.Interfaces;
using User.Domain.Entities;
using User.Domain.Errors;
using System.Linq.Dynamic.Core;


namespace User.Infrastructure.Services;


public class UserService(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager) : IUserService
{
    public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure<UserProfileResponse>(UserErrors.UserNotFound);

        return Result.Success(new UserProfileResponse(
            user.Email!, user.UserName!, user.FirstName, user.LastName));
    }

    public async Task<Result> UpdateProfileAsync(string userId, string firstName, string lastName)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        user.FirstName = firstName;
        user.LastName = lastName;

        await userManager.UpdateAsync(user);

        return Result.Success();
    }

    public async Task<Result> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Result<PaginatedList<UserResponse>>> GetAllAsync(
        int pageNumber, int pageSize, string? searchValue,
        string? sortColumn, string? sortDirection,
        CancellationToken cancellationToken = default)
    {
        var query = userManager.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchValue))
        {
            query = query.Where(u =>
                u.FirstName.Contains(searchValue) ||
                u.LastName.Contains(searchValue) ||
                u.Email!.Contains(searchValue));
        }

        if (!string.IsNullOrWhiteSpace(sortColumn))
        {
            var direction = sortDirection?.ToLower() == "desc" ? "descending" : "ascending";
            query = query.OrderBy($"{sortColumn} {direction}");
        }

        var paginatedUsers = await PaginatedList<ApplicationUser>.CreateAsync(
            query, pageNumber, pageSize, cancellationToken);

        var userResponses = new List<UserResponse>();

        foreach (var user in paginatedUsers.Items)
        {
            var roles = await userManager.GetRolesAsync(user);
            userResponses.Add(new UserResponse(
                user.Id, user.FirstName, user.LastName,
                user.Email!, user.IsDisabled, roles));
        }

        return Result.Success(new PaginatedList<UserResponse>(
            userResponses, paginatedUsers.PageNumber, paginatedUsers.TotalPages));
    }
    public async Task<Result<UserResponse>> GetByIdAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure<UserResponse>(UserErrors.UserNotFound);

        var roles = await userManager.GetRolesAsync(user);

        return Result.Success(new UserResponse(
            user.Id, user.FirstName, user.LastName, user.Email!,
            user.IsDisabled, roles));
    }

    public async Task<Result> CreateUserAsync(string firstName, string lastName, string email, string password, IEnumerable<string> roles)
    {
        var emailExists = await userManager.Users.AnyAsync(u => u.Email == email);

        if (emailExists)
            return Result.Failure(UserErrors.DuplicatedEmail);

        var validRoles = await ValidateRolesAsync(roles);
        if (!validRoles)
            return Result.Failure(UserErrors.InvalidRoles);

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        await userManager.AddToRolesAsync(user, roles);

        return Result.Success();
    }

    public async Task<Result> UpdateUserAsync(string userId, string firstName, string lastName, IEnumerable<string> roles)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        var validRoles = await ValidateRolesAsync(roles);
        if (!validRoles)
            return Result.Failure(UserErrors.InvalidRoles);

        user.FirstName = firstName;
        user.LastName = lastName;

        await userManager.UpdateAsync(user);

        var currentRoles = await userManager.GetRolesAsync(user);
        await userManager.RemoveFromRolesAsync(user, currentRoles);
        await userManager.AddToRolesAsync(user, roles);

        return Result.Success();
    }

    public async Task<Result> ToggleStatusAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(UserErrors.UserNotFound);

        user.IsDisabled = !user.IsDisabled;
        await userManager.UpdateAsync(user);

        return Result.Success();
    }

    private async Task<bool> ValidateRolesAsync(IEnumerable<string> roles)
    {
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                return false;
        }
        return true;
    }
}