



namespace Shopsy.BuildingBlocks.Abstractions;

public record class Error(string Code, string Description, int? StatusCode = null)
{
    public static readonly Error None = new(string.Empty, string.Empty, null);
    public static readonly Error Unknown = new("General.Unknown", "An unexpected error occurred", 500);


    public static Error Validation(string code, string description) =>
       new(code, description, 400);

    public static Error NotFound(string code, string description) =>
        new(code, description, 404);

    public static Error Conflict(string code, string description) =>
        new(code, description, 409);

    public static Error Unauthorized(string code, string description) =>
        new(code, description, 401);

    public static Error Forbidden(string code, string description) =>
        new(code, description, 403);

}
