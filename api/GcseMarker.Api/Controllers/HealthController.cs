using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GcseMarker.Api.Data;

namespace GcseMarker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public HealthController(AppDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }

    [HttpGet("ready")]
    public async Task<IActionResult> Ready()
    {
        var dbConnected = await _dbContext.Database.CanConnectAsync();
        var anthropicConfigured = !string.IsNullOrEmpty(_configuration["Anthropic:ApiKey"]);

        return Ok(new
        {
            status = dbConnected ? "ready" : "degraded",
            database = dbConnected ? "connected" : "disconnected",
            anthropicApi = anthropicConfigured ? "configured" : "not configured"
        });
    }
}
