namespace GcseMarker.Api.Models.DTOs;

public record MarkEssayResponse(bool Success, FeedbackDto? Feedback, string? Error);
