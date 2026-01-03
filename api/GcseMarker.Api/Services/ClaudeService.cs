using System.Text;
using System.Text.Json;
using GcseMarker.Api.Data.Skills;
using GcseMarker.Api.Models.Claude;

namespace GcseMarker.Api.Services;

public class ClaudeService : IClaudeService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ClaudeService> _logger;
    private const string ApiUrl = "https://api.anthropic.com/v1/messages";
    private const string Model = "claude-opus-4-5-20251101";
    private const int ThinkingBudgetTokens = 10000;
    private const int MaxTokens = 16000;

    public ClaudeService(HttpClient httpClient, ILogger<ClaudeService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> MarkEssayAsync(SkillDefinition skill, string question, string pdfBase64, CancellationToken cancellationToken = default)
    {
        var systemPrompt = BuildSystemPrompt(skill);

        var request = new
        {
            model = Model,
            max_tokens = MaxTokens,
            thinking = new
            {
                type = "enabled",
                budget_tokens = ThinkingBudgetTokens
            },
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = new object[]
                    {
                        new { type = "text", text = systemPrompt },
                        new
                        {
                            type = "document",
                            source = new
                            {
                                type = "base64",
                                media_type = "application/pdf",
                                data = pdfBase64
                            }
                        },
                        new
                        {
                            type = "text",
                            text = $"""
                                Please mark this essay according to the GCSE marking criteria provided.

                                ## Essay Question
                                {question}

                                Assess how well the student has addressed this specific question in their response.
                                Provide deep, insightful analysis with specific examples from the student's work.
                                Analyse individual word choices where relevant.
                                """
                        }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _logger.LogInformation("Sending marking request to Claude API for skill {SkillId}", skill.Id);

        var response = await _httpClient.PostAsync(ApiUrl, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonSerializer.Deserialize<ClaudeResponse>(responseJson);

        var textBlock = result?.Content?.FirstOrDefault(c => c.Type == "text");
        return textBlock?.Text ?? "Unable to generate feedback";
    }

    private static string BuildSystemPrompt(SkillDefinition skill)
    {
        return $"""
            {skill.SystemPrompt}

            ## Reference: Mark Scheme
            {skill.MarkScheme}

            ## Reference: Essay Techniques
            {skill.EssayTechniques}

            ## Important: Depth of Analysis

            When marking, ensure you provide:
            1. **Word-level analysis** - Pick specific words from quotes and explain why the writer chose *that* word over alternatives
            2. **Precise terminology** - Use terms like "stichomythia," "semantic field," "euphemism" where appropriate
            3. **Original interpretations** - Go beyond standard readings to offer perceptive insights
            4. **Specific examples** - Quote directly from the student's work and analyse in depth
            5. **Actionable targets** - Give concrete steps the student can take to improve

            Take your time to read the essay carefully and think deeply about the analysis before responding.
            """;
    }
}
