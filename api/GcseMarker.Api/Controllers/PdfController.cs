using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GcseMarker.Api.Models.DTOs;
using GcseMarker.Api.Services;

namespace GcseMarker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PdfController : ControllerBase
{
    private readonly IPdfService _pdfService;
    private readonly ILogger<PdfController> _logger;

    public PdfController(IPdfService pdfService, ILogger<PdfController> logger)
    {
        _pdfService = pdfService;
        _logger = logger;
    }

    [HttpPost("feedback")]
    public IActionResult GenerateFeedbackPdf([FromBody] FeedbackPdfRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FeedbackMarkdown))
        {
            return BadRequest(new { error = "Feedback markdown is required" });
        }

        if (string.IsNullOrWhiteSpace(request.GradeBand))
        {
            return BadRequest(new { error = "Grade band is required" });
        }

        if (string.IsNullOrWhiteSpace(request.SkillName))
        {
            return BadRequest(new { error = "Skill name is required" });
        }

        try
        {
            _logger.LogInformation("Generating PDF for skill {SkillName}, grade {GradeBand}",
                request.SkillName, request.GradeBand);

            var pdfBytes = _pdfService.GenerateFeedbackPdf(request);

            var fileName = $"feedback-{SanitizeFileName(request.SkillName)}-grade-{request.GradeBand}-{request.Timestamp:yyyy-MM-dd}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate PDF");
            return StatusCode(500, new { error = "Failed to generate PDF" });
        }
    }

    private static string SanitizeFileName(string name)
    {
        var invalid = Path.GetInvalidFileNameChars();
        return string.Join("", name.Split(invalid, StringSplitOptions.RemoveEmptyEntries))
            .Replace(" ", "-")
            .ToLowerInvariant();
    }
}
