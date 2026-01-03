namespace GcseMarker.Api.Models.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string? Name { get; set; }
    public bool Enabled { get; set; } = true;
    public bool IsAdmin { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public ICollection<UsageLog> UsageLogs { get; set; } = new List<UsageLog>();
}
