using GcseMarker.Api.Models.DTOs;

namespace GcseMarker.Api.Services;

public interface IPdfService
{
    byte[] GenerateFeedbackPdf(FeedbackPdfRequest request);
}
