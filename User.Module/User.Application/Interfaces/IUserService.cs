

using Shopsy.BuildingBlocks.Abstractions;
using User.Application.DtoContracts;

namespace User.Application.Interfaces;

public interface IUserService
{
    Task<Result<UserProfileResponse>> GetProfileAsync(string userId);

    Task<Result> UpdateProfileAsync(string userId, string firstName, string lastName);

    Task<Result> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

    Task<Result<PaginatedList<UserResponse>>> GetAllAsync(int pageNumber, int pageSize, string? searchValue, string? sortColumn, string? sortDirection, CancellationToken cancellationToken = default);

    Task<Result<UserResponse>> GetByIdAsync(string userId);

    Task<Result> CreateUserAsync(string firstName, string lastName, string email, string password, IEnumerable<string> roles);

    Task<Result> UpdateUserAsync(string userId, string firstName, string lastName, IEnumerable<string> roles);

    Task<Result> ToggleStatusAsync(string userId);
}