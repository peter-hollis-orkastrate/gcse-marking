using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using GcseMarker.Api.Controllers;
using GcseMarker.Api.Data;
using GcseMarker.Api.Data.Skills;
using GcseMarker.Api.Models.DTOs;
using GcseMarker.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace GcseMarker.Api.Tests.Controllers;

public class MarkControllerTests
{
    private readonly Mock<IClaudeService> _claudeServiceMock;
    private readonly Mock<ISkillsService> _skillsServiceMock;
    private readonly Mock<ILogger<MarkController>> _loggerMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly AppDbContext _dbContext;

    public MarkControllerTests()
    {
        _claudeServiceMock = new Mock<IClaudeService>();
        _skillsServiceMock = new Mock<ISkillsService>();
        _loggerMock = new Mock<ILogger<MarkController>>();
        _configurationMock = new Mock<IConfiguration>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new AppDbContext(options);
    }

    private MarkController CreateController(ClaimsPrincipal? user = null)
    {
        var controller = new MarkController(
            _claudeServiceMock.Object,
            _skillsServiceMock.Object,
            _dbContext,
            _loggerMock.Object,
            _configurationMock.Object);

        if (user != null)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        return controller;
    }

    [Fact]
    public async Task MarkEssay_ReturnsBadRequest_WhenSkillNotFound()
    {
        // Arrange
        _skillsServiceMock.Setup(s => s.GetSkillById("invalid")).Returns((SkillDefinition?)null);
        var controller = CreateController();
        var request = new MarkEssayRequest("invalid", "Question", "base64pdf");

        // Act
        var result = await controller.MarkEssay(request);

        // Assert
        var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequest.Value.Should().BeOfType<MarkEssayResponse>().Subject;
        response.Success.Should().BeFalse();
        response.Error.Should().Contain("Invalid skill ID");
    }

    [Fact]
    public async Task MarkEssay_ReturnsBadRequest_WhenQuestionEmpty()
    {
        // Arrange
        _skillsServiceMock.Setup(s => s.GetSkillById("frankenstein")).Returns(FrankensteinSkill.Definition);
        var controller = CreateController();
        var request = new MarkEssayRequest("frankenstein", "", "base64pdf");

        // Act
        var result = await controller.MarkEssay(request);

        // Assert
        var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequest.Value.Should().BeOfType<MarkEssayResponse>().Subject;
        response.Error.Should().Contain("question is required");
    }

    [Fact]
    public async Task MarkEssay_ReturnsBadRequest_WhenPdfEmpty()
    {
        // Arrange
        _skillsServiceMock.Setup(s => s.GetSkillById("frankenstein")).Returns(FrankensteinSkill.Definition);
        var controller = CreateController();
        var request = new MarkEssayRequest("frankenstein", "Test question", "");

        // Act
        var result = await controller.MarkEssay(request);

        // Assert
        var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequest.Value.Should().BeOfType<MarkEssayResponse>().Subject;
        response.Error.Should().Contain("PDF file is required");
    }

    [Fact]
    public async Task MarkEssay_ReturnsBadRequest_WhenPdfTooLarge()
    {
        // Arrange
        _skillsServiceMock.Setup(s => s.GetSkillById("frankenstein")).Returns(FrankensteinSkill.Definition);
        var controller = CreateController();
        var largePdf = new string('x', 16_000_000);
        var request = new MarkEssayRequest("frankenstein", "Test question", largePdf);

        // Act
        var result = await controller.MarkEssay(request);

        // Assert
        var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequest.Value.Should().BeOfType<MarkEssayResponse>().Subject;
        response.Error.Should().Contain("too large");
    }

    [Fact]
    public async Task MarkEssay_ReturnsError_WhenApiKeyNotConfigured()
    {
        // Arrange
        _skillsServiceMock.Setup(s => s.GetSkillById("frankenstein")).Returns(FrankensteinSkill.Definition);
        _configurationMock.Setup(c => c["Anthropic:ApiKey"]).Returns((string?)null);
        var controller = CreateController();
        var request = new MarkEssayRequest("frankenstein", "Test question", "base64pdf");

        // Act
        var result = await controller.MarkEssay(request);

        // Assert
        var statusResult = result.Should().BeOfType<ObjectResult>().Subject;
        statusResult.StatusCode.Should().Be(500);
    }

    [Fact]
    public async Task MarkEssay_ReturnsSuccess_WhenAllValid()
    {
        // Arrange
        var feedback = "### Overall Grade Band: 6-7\n\nGreat work!";
        _skillsServiceMock.Setup(s => s.GetSkillById("frankenstein")).Returns(FrankensteinSkill.Definition);
        _configurationMock.Setup(c => c["Anthropic:ApiKey"]).Returns("test-key");
        _claudeServiceMock.Setup(c => c.MarkEssayAsync(
            It.IsAny<SkillDefinition>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(feedback);

        var claims = new List<Claim>
        {
            new Claim("UserId", "1"),
            new Claim(ClaimTypes.Email, "test@example.com")
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);

        var controller = CreateController(user);
        var request = new MarkEssayRequest("frankenstein", "Test question", "base64pdf");

        // Act
        var result = await controller.MarkEssay(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var response = okResult.Value.Should().BeOfType<MarkEssayResponse>().Subject;
        response.Success.Should().BeTrue();
        response.Feedback.Should().NotBeNull();
        response.Feedback!.GradeBand.Should().Be("6-7");
    }
}
