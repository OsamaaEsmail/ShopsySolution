namespace Order.Application.DtoContracts;

public record AddressResponse(
    Guid Id,
    string FullName,
    string PhoneNumber,
    string EmailAddress,
    string Address,
    string Country,
    string StateOrProvince,
    string City,
    string PostalOrZipCode);