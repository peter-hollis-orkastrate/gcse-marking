using System.Text.Json.Serialization;

namespace GcseMarker.Api.Models.Claude;

public class ClaudeResponse
{
    [JsonPropertyName("content")]
    public List<ContentBlock>? Content { get; set; }
}

public class ContentBlock
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }
}
