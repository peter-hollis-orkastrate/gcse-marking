using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using GcseMarker.Api.Controllers;
using GcseMarker.Api.Models.DTOs;
using GcseMarker.Api.Services;
using GcseMarker.Api.Data.Skills;

namespace GcseMarker.Api.Tests.Controllers;

public class SkillsControllerTests
{
    [Fact]
    public void GetSkills_ReturnsAllSkills()
    {
        // Arrange
        var mockService = new Mock<ISkillsService>();
        mockService.Setup(s => s.GetAllSkills()).Returns(new[]
        {
            new SkillDto("frankenstein", "AQA - English Literature - Frankenstein", "Description", "English Literature"),
            new SkillDto("macbeth", "AQA - English Literature - Macbeth", "Description", "English Literature")
        });

        var controller = new SkillsController(mockService.Object);

        // Act
        var result = controller.GetSkills();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().NotBeNull();
    }

    [Fact]
    public void GetSkill_ReturnsNotFound_WhenSkillDoesNotExist()
    {
        // Arrange
        var mockService = new Mock<ISkillsService>();
        mockService.Setup(s => s.GetSkillById("nonexistent")).Returns((SkillDefinition?)null);

        var controller = new SkillsController(mockService.Object);

        // Act
        var result = controller.GetSkill("nonexistent");

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public void GetSkill_ReturnsSkill_WhenSkillExists()
    {
        // Arrange
        var mockService = new Mock<ISkillsService>();
        mockService.Setup(s => s.GetSkillById("frankenstein")).Returns(FrankensteinSkill.Definition);

        var controller = new SkillsController(mockService.Object);

        // Act
        var result = controller.GetSkill("frankenstein");

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var skill = okResult.Value.Should().BeOfType<SkillDefinition>().Subject;
        skill.Id.Should().Be("frankenstein");
    }
}
