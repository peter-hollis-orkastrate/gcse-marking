using Microsoft.AspNetCore.Mvc;
using GcseMarker.Api.Services;

namespace GcseMarker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillsController : ControllerBase
{
    private readonly ISkillsService _skillsService;

    public SkillsController(ISkillsService skillsService)
    {
        _skillsService = skillsService;
    }

    [HttpGet]
    public IActionResult GetSkills()
    {
        var skills = _skillsService.GetAllSkills();
        return Ok(new { skills });
    }

    [HttpGet("{skillId}")]
    public IActionResult GetSkill(string skillId)
    {
        var skill = _skillsService.GetSkillById(skillId);
        if (skill == null)
            return NotFound(new { error = "Skill not found" });

        return Ok(skill);
    }
}
