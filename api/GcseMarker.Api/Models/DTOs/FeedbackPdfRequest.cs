namespace GcseMarker.Api.Models.DTOs;

public record FeedbackPdfRequest(
    string GradeBand,
    string? Question,
    string FeedbackMarkdown,
    string SkillName,
    DateTime Timestamp
);
