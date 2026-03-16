



using Microsoft.AspNetCore.Identity.Data;
using Shopsy.BuildingBlocks.Abstractions;
using User.Application.DtoContracts;

namespace User.Application.Interfaces;


public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);

    Task<Result> RegisterAsync(string firstName, string lastName, string email, string password, CancellationToken cancellationToken = default);

    Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);

    Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);

    Task<Result> ConfirmEmailAsync(string userId, string code);

    Task<Result> ResendConfirmationEmailAsync(string email);

    Task<Result> SendResetPasswordCodeAsync(string email);

    Task<Result> ResetPasswordAsync(string email, string code, string newPassword);
}