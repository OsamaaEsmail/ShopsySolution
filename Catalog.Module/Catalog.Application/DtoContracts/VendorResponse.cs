namespace Catalog.Application.DtoContracts;

public record VendorResponse(
    Guid Id,
    string VendorName,
    string Email,
    string PhoneNumber,
    string? VendorPicUrl);