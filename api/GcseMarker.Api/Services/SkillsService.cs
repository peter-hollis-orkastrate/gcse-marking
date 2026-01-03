using GcseMarker.Api.Data.Skills;
using GcseMarker.Api.Models.DTOs;

namespace GcseMarker.Api.Services;

public class SkillsService : ISkillsService
{
    public IEnumerable<SkillDto> GetAllSkills()
    {
        return SkillsRepository.GetAll()
            .Select(s => new SkillDto(s.Id, s.Name, s.Description, s.Subject));
    }

    public SkillDefinition? GetSkillById(string id)
    {
        return SkillsRepository.GetById(id);
    }
}
