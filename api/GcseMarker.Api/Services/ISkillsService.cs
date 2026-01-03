using GcseMarker.Api.Data.Skills;
using GcseMarker.Api.Models.DTOs;

namespace GcseMarker.Api.Services;

public interface ISkillsService
{
    IEnumerable<SkillDto> GetAllSkills();
    SkillDefinition? GetSkillById(string id);
}
