using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using GcseMarker.Api.Data;
using GcseMarker.Api.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Services
builder.Services.AddScoped<ISkillsService, SkillsService>();

// Add Claude HttpClient with 5 minute timeout
builder.Services.AddHttpClient<IClaudeService, ClaudeService>(client =>
{
    client.BaseAddress = new Uri("https://api.anthropic.com/");
    client.DefaultRequestHeaders.Add("x-api-key", builder.Configuration["Anthropic:ApiKey"]);
    client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
    client.Timeout = TimeSpan.FromMinutes(5); // Extended for thinking model
});

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    var cookieDomain = builder.Configuration["CookieDomain"];
    if (!string.IsNullOrEmpty(cookieDomain))
        options.Cookie.Domain = cookieDomain;

    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Google:ClientId"] ?? "";
    options.ClientSecret = builder.Configuration["Google:ClientSecret"] ?? "";
    options.Events.OnCreatingTicket = async context =>
    {
        var email = context.Principal?.FindFirstValue(ClaimTypes.Email);
        var dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email && u.Enabled);

        if (user == null)
        {
            context.Fail("User not authorized");
            return;
        }

        user.LastLoginAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();

        var identity = (ClaimsIdentity)context.Principal!.Identity!;
        identity.AddClaim(new Claim("UserId", user.Id.ToString()));
        identity.AddClaim(new Claim("IsAdmin", user.IsAdmin.ToString()));
    };
});

// Add CORS
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
    ?? throw new InvalidOperationException("AllowedOrigins configuration is required");
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(allowedOrigins)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Make Program accessible for testing
public partial class Program { }
