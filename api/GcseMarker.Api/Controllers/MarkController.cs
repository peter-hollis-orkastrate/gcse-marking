using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GcseMarker.Api.Data;
using GcseMarker.Api.Models.DTOs;
using GcseMarker.Api.Models.Entities;
using GcseMarker.Api.Services;

namespace GcseMarker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MarkController : ControllerBase
{
    private readonly IClaudeService _claudeService;
    private readonly ISkillsService _skillsService;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<MarkController> _logger;
    private readonly IConfiguration _configuration;

    public MarkController(
        IClaudeService claudeService,
        ISkillsService skillsService,
        AppDbContext dbContext,
        ILogger<MarkController> logger,
        IConfiguration configuration)
    {
        _claudeService = claudeService;
        _skillsService = skillsService;
        _dbContext = dbContext;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> MarkEssay([FromBody] MarkEssayRequest request)
    {
        // Validate skill
        var skill = _skillsService.GetSkillById(request.SkillId);
        if (skill == null)
            return BadRequest(new MarkEssayResponse(false, null, "Invalid skill ID"));

        // Validate question
        if (string.IsNullOrWhiteSpace(request.Question))
            return BadRequest(new MarkEssayResponse(false, null, "Essay question is required"));

        // Validate PDF
        if (string.IsNullOrWhiteSpace(request.PdfBase64))
            return BadRequest(new MarkEssayResponse(false, null, "PDF file is required"));

        // Check PDF size (~10MB limit)
        if (request.PdfBase64.Length > 15_000_000)
            return BadRequest(new MarkEssayResponse(false, null, "PDF file is too large. Maximum size is 10MB."));

        // Check API key configured
        if (string.IsNullOrEmpty(_configuration["Anthropic:ApiKey"]))
        {
            _logger.LogError("ANTHROPIC_API_KEY is not configured");
            return StatusCode(500, new MarkEssayResponse(false, null, "AI service not configured. Contact administrator."));
        }

        try
        {
            var feedback = await _claudeService.MarkEssayAsync(skill, request.Question, request.PdfBase64);

            // Log usage
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (int.TryParse(userIdClaim, out var userId))
            {
                _dbContext.UsageLogs.Add(new UsageLog
                {
                    UserId = userId,
                    SkillUsed = request.SkillId
                });
                await _dbContext.SaveChangesAsync();
            }

            // Extract grade band
            var gradeBandMatch = Regex.Match(feedback, @"Grade Band:\s*(\d-\d)", RegexOptions.IgnoreCase);
            var gradeBand = gradeBandMatch.Success ? gradeBandMatch.Groups[1].Value : "Unknown";

            return Ok(new MarkEssayResponse(true, new FeedbackDto(feedback, gradeBand, DateTime.UtcNow), null));
        }
        catch (HttpRequestException ex) when (ex.Message.Contains("rate limit", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning(ex, "Claude API rate limit hit");
            return StatusCode(429, new MarkEssayResponse(false, null, "AI service is busy. Please try again in a few minutes."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking essay");
            return StatusCode(500, new MarkEssayResponse(false, null, "Failed to mark essay. Please try again."));
        }
    }
}
