namespace GcseMarker.Api.Data.Skills;

public static class SkillsRepository
{
    private static readonly Dictionary<string, SkillDefinition> _skills = new()
    {
        { "frankenstein", FrankensteinSkill.Definition },
        { "macbeth", MacbethSkill.Definition },
        { "lord-of-the-flies", LordOfTheFliesSkill.Definition }
    };

    public static IEnumerable<SkillDefinition> GetAll() => _skills.Values.OrderBy(s => s.Name);

    public static SkillDefinition? GetById(string id) =>
        _skills.TryGetValue(id, out var skill) ? skill : null;
}
