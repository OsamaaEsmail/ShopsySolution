




namespace User.Application.DtoContracts;

public record class AuthResponse
(
    string Id,
    string? Email,
    string FirstName,
    string LastName,
    string Token,
    int ExpiresIn,
    string RefreshToken,
    DateTime RefreshTokenExpiryDate

);
