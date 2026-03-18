



namespace Shopsy.BuildingBlocks.SharedExtensions;

public abstract class AuditableEntity
{
    public string CreatedById { get; set; } = string.Empty;
    public string CreatedByName { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public string? UpdatedById { get; set; }
    public string? UpdatedByName { get; set; }
    public DateTime? UpdatedOn { get; set; }
}