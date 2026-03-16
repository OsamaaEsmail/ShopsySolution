


using System.ComponentModel.DataAnnotations;

namespace User.Infrastructure.Authentication;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required]
    public string Key { get; init; } = string.Empty;

    [Required]
    public string Issuer { get; init; } = string.Empty;

    [Required]
    public string Audience { get; init; } = string.Empty;

    [Range(10, int.MaxValue, ErrorMessage = "Invalid ExpiryMinutes")]
    public int ExpiryMinutes { get; init; }
}

