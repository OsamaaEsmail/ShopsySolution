



namespace Catalog.Domain.Entities;


public class Vendor
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string VendorName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? VendorPicUrl { get; set; }
}