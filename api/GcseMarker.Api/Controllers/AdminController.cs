using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GcseMarker.Api.Data;

namespace GcseMarker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    private bool IsAdmin()
    {
        var isAdminClaim = User.FindFirst("IsAdmin")?.Value;
        return bool.TryParse(isAdminClaim, out var isAdmin) && isAdmin;
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        if (!IsAdmin())
            return Forbid();

        var users = await _context.Users
            .Include(u => u.UsageLogs)
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.Name,
                u.Enabled,
                u.IsAdmin,
                u.CreatedAt,
                u.LastLoginAt,
                UsageCount = u.UsageLogs.Count
            })
            .ToListAsync();

        return Ok(new { users });
    }

    [HttpPost("users")]
    public async Task<IActionResult> AddUser([FromBody] AddUserRequest request)
    {
        if (!IsAdmin())
            return Forbid();

        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest(new { error = "Email is required" });

        var existing = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existing != null)
            return Conflict(new { error = "User with this email already exists" });

        var user = new Models.Entities.User
        {
            Email = request.Email,
            Name = request.Name,
            Enabled = true,
            IsAdmin = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            user = new { user.Id, user.Email, user.Name }
        });
    }

    [HttpPatch("users/{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        if (!IsAdmin())
            return Forbid();

        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound(new { error = "User not found" });

        if (request.Enabled.HasValue)
            user.Enabled = request.Enabled.Value;

        if (request.IsAdmin.HasValue)
            user.IsAdmin = request.IsAdmin.Value;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            success = true,
            user = new { user.Id, user.Email, user.Enabled, user.IsAdmin }
        });
    }

    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (!IsAdmin())
            return Forbid();

        var currentUserId = GetCurrentUserId();
        if (id == currentUserId)
            return BadRequest(new { error = "Cannot delete your own account" });

        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound(new { error = "User not found" });

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok(new { success = true });
    }

    [HttpGet("usage")]
    public async Task<IActionResult> GetUsage([FromQuery] int days = 30)
    {
        if (!IsAdmin())
            return Forbid();

        var since = DateTime.UtcNow.AddDays(-days);

        var totalMarks = await _context.UsageLogs
            .Where(u => u.CreatedAt >= since)
            .CountAsync();

        var uniqueUsers = await _context.UsageLogs
            .Where(u => u.CreatedAt >= since)
            .Select(u => u.UserId)
            .Distinct()
            .CountAsync();

        var bySkill = await _context.UsageLogs
            .Where(u => u.CreatedAt >= since)
            .GroupBy(u => u.SkillUsed)
            .Select(g => new { SkillId = g.Key, Count = g.Count() })
            .OrderByDescending(s => s.Count)
            .ToListAsync();

        var topUsers = await _context.UsageLogs
            .Where(u => u.CreatedAt >= since)
            .GroupBy(u => u.UserId)
            .Select(g => new { UserId = g.Key, Count = g.Count() })
            .OrderByDescending(u => u.Count)
            .Take(10)
            .ToListAsync();

        var topUsersWithEmail = new List<object>();
        foreach (var u in topUsers)
        {
            var user = await _context.Users.FindAsync(u.UserId);
            topUsersWithEmail.Add(new { Email = user?.Email ?? "Unknown", u.Count });
        }

        return Ok(new
        {
            summary = new
            {
                totalMarks,
                uniqueUsers,
                periodDays = days
            },
            bySkill = bySkill.Select(s => new { s.SkillId, s.Count }),
            topUsers = topUsersWithEmail
        });
    }
}

public record AddUserRequest(string Email, string? Name);
public record UpdateUserRequest(bool? Enabled, bool? IsAdmin);
