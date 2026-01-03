using GcseMarker.Api.Data.Skills;

namespace GcseMarker.Api.Services;

public interface IClaudeService
{
    Task<string> MarkEssayAsync(SkillDefinition skill, string question, string pdfBase64, CancellationToken cancellationToken = default);
}
