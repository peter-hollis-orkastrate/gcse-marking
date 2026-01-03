namespace GcseMarker.Api.Models.Entities;

public class UsageLog
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string SkillUsed { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User User { get; set; } = null!;
}
