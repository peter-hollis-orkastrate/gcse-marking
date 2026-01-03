namespace GcseMarker.Api.Data.Skills;

public class SkillDefinition
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Subject { get; init; }
    public required string SystemPrompt { get; init; }
    public required string MarkScheme { get; init; }
    public required string EssayTechniques { get; init; }
}
