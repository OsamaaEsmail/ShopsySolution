namespace Order.Domain.Entities;

public class ShippingAddress
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string StateOrProvince { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalOrZipCode { get; set; } = string.Empty;
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
}