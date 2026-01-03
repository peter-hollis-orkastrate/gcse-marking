using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GcseMarker.Api.Models.DTOs;

namespace GcseMarker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly string _frontendUrl;

    public AuthController(IConfiguration configuration)
    {
        _frontendUrl = configuration["FrontendUrl"]
            ?? throw new InvalidOperationException("FrontendUrl configuration is required");
    }

    [HttpGet("google")]
    public IActionResult GoogleLogin()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = "/api/auth/google/callback"
        };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        if (!result.Succeeded)
            return Redirect($"{_frontendUrl}?error=auth_failed");

        return Redirect(_frontendUrl);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var name = User.FindFirst(ClaimTypes.Name)?.Value;
        var userId = User.FindFirst("UserId")?.Value;
        var isAdmin = User.FindFirst("IsAdmin")?.Value;

        if (email == null || userId == null)
            return Unauthorized();

        return Ok(new UserDto(
            int.Parse(userId),
            email,
            name,
            bool.TryParse(isAdmin, out var admin) && admin
        ));
    }
}
