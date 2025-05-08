namespace SampleApi.Entity;

public abstract class Auditable
{
    public string CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}