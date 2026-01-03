using System.Net;
using System.Text.Json;
using Moq;
using Moq.Protected;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using GcseMarker.Api.Services;
using GcseMarker.Api.Data.Skills;

namespace GcseMarker.Api.Tests.Services;

public class ClaudeServiceTests
{
    private readonly Mock<ILogger<ClaudeService>> _loggerMock;

    public ClaudeServiceTests()
    {
        _loggerMock = new Mock<ILogger<ClaudeService>>();
    }

    [Fact]
    public async Task MarkEssayAsync_ReturnsText_WhenApiSucceeds()
    {
        // Arrange
        var expectedFeedback = "### Overall Grade Band: 6-7\n\nGreat essay!";
        var mockResponse = new
        {
            content = new[]
            {
                new { type = "thinking", text = "Analyzing..." },
                new { type = "text", text = expectedFeedback }
            }
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(mockResponse))
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://api.anthropic.com/")
        };

        var service = new ClaudeService(httpClient, _loggerMock.Object);
        var skill = FrankensteinSkill.Definition;

        // Act
        var result = await service.MarkEssayAsync(skill, "Test question", "base64pdf");

        // Assert
        result.Should().Be(expectedFeedback);
    }

    [Fact]
    public async Task MarkEssayAsync_ThrowsException_WhenApiFails()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://api.anthropic.com/")
        };

        var service = new ClaudeService(httpClient, _loggerMock.Object);
        var skill = MacbethSkill.Definition;

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() =>
            service.MarkEssayAsync(skill, "Test question", "base64pdf"));
    }

    [Fact]
    public async Task MarkEssayAsync_ReturnsDefaultMessage_WhenNoTextContent()
    {
        // Arrange
        var mockResponse = new
        {
            content = new[]
            {
                new { type = "thinking", text = "Analyzing..." }
            }
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(mockResponse))
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://api.anthropic.com/")
        };

        var service = new ClaudeService(httpClient, _loggerMock.Object);
        var skill = LordOfTheFliesSkill.Definition;

        // Act
        var result = await service.MarkEssayAsync(skill, "Test question", "base64pdf");

        // Assert
        result.Should().Be("Unable to generate feedback");
    }
}
