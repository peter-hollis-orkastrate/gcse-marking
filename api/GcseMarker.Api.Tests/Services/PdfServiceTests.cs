using FluentAssertions;
using GcseMarker.Api.Models.DTOs;
using GcseMarker.Api.Services;

namespace GcseMarker.Api.Tests.Services;

public class PdfServiceTests
{
    private readonly PdfService _pdfService;
    private readonly MarkdownParser _markdownParser;

    public PdfServiceTests()
    {
        _markdownParser = new MarkdownParser();
        _pdfService = new PdfService(_markdownParser);
    }

    [Fact]
    public void GenerateFeedbackPdf_ReturnsPdfBytes_WithValidRequest()
    {
        // Arrange
        var request = new FeedbackPdfRequest(
            GradeBand: "6-7",
            Question: "How does Shakespeare present Macbeth's ambition?",
            FeedbackMarkdown: "## Summary\n\nGreat essay with clear analysis.",
            SkillName: "Macbeth",
            Timestamp: new DateTime(2025, 1, 15, 10, 30, 0)
        );

        // Act
        var result = _pdfService.GenerateFeedbackPdf(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Length.Should().BeGreaterThan(100);
    }

    [Fact]
    public void GenerateFeedbackPdf_CreatesPdf_WithPdfMagicNumber()
    {
        // Arrange
        var request = new FeedbackPdfRequest(
            GradeBand: "5",
            Question: null,
            FeedbackMarkdown: "# Feedback\n\nWell done!",
            SkillName: "Frankenstein",
            Timestamp: DateTime.UtcNow
        );

        // Act
        var result = _pdfService.GenerateFeedbackPdf(request);

        // Assert
        // PDF files start with %PDF
        result[0].Should().Be(0x25); // %
        result[1].Should().Be(0x50); // P
        result[2].Should().Be(0x44); // D
        result[3].Should().Be(0x46); // F
    }

    [Fact]
    public void GenerateFeedbackPdf_HandlesMissingQuestion()
    {
        // Arrange
        var request = new FeedbackPdfRequest(
            GradeBand: "8-9",
            Question: null,
            FeedbackMarkdown: "Excellent work!",
            SkillName: "Lord of the Flies",
            Timestamp: DateTime.UtcNow
        );

        // Act
        var result = _pdfService.GenerateFeedbackPdf(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public void GenerateFeedbackPdf_HandlesEmptyQuestion()
    {
        // Arrange
        var request = new FeedbackPdfRequest(
            GradeBand: "4",
            Question: "",
            FeedbackMarkdown: "### Strengths\n\n- Good structure",
            SkillName: "A Christmas Carol",
            Timestamp: DateTime.UtcNow
        );

        // Act
        var result = _pdfService.GenerateFeedbackPdf(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public void GenerateFeedbackPdf_HandlesComplexMarkdown()
    {
        // Arrange
        var complexMarkdown = @"# Overall Assessment

## Grade Band: 7-8

Your essay demonstrates a **strong understanding** of the text.

### Key Strengths

- Clear thesis statement
- Good use of quotations
- *Effective* analysis of language

---

### Areas for Improvement

- More detailed context needed
- Consider alternative interpretations

## Conclusion

A well-structured response that shows genuine insight.";

        var request = new FeedbackPdfRequest(
            GradeBand: "7-8",
            Question: "Explore the theme of ambition in Macbeth.",
            FeedbackMarkdown: complexMarkdown,
            SkillName: "Macbeth",
            Timestamp: new DateTime(2025, 1, 15, 14, 45, 0)
        );

        // Act
        var result = _pdfService.GenerateFeedbackPdf(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Length.Should().BeGreaterThan(1000); // Complex content should produce larger PDF
    }

    [Fact]
    public void GenerateFeedbackPdf_HandlesLongFeedback()
    {
        // Arrange
        var longFeedback = string.Join("\n\n", Enumerable.Range(1, 20).Select(i =>
            $"## Section {i}\n\nThis is paragraph {i} with some detailed feedback about the essay. " +
            $"The student has demonstrated understanding but could improve in certain areas. " +
            $"Consider reviewing the following points carefully."));

        var request = new FeedbackPdfRequest(
            GradeBand: "5-6",
            Question: "Analyse the significance of the monster in Frankenstein.",
            FeedbackMarkdown: longFeedback,
            SkillName: "Frankenstein",
            Timestamp: DateTime.UtcNow
        );

        // Act
        var result = _pdfService.GenerateFeedbackPdf(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public void GenerateFeedbackPdf_HandlesSpecialCharacters()
    {
        // Arrange
        var request = new FeedbackPdfRequest(
            GradeBand: "6",
            Question: "How does Shelley use the creature's \"otherness\" to explore themes?",
            FeedbackMarkdown: "The essay explores themes of 'isolation' and \"identity\" effectively. " +
                "Use of em-dash — and other punctuation: semi-colons; colons: work well.",
            SkillName: "Frankenstein",
            Timestamp: DateTime.UtcNow
        );

        // Act
        var result = _pdfService.GenerateFeedbackPdf(request);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public void GenerateFeedbackPdf_ProducesConsistentOutput()
    {
        // Arrange
        var request = new FeedbackPdfRequest(
            GradeBand: "7",
            Question: "Test question",
            FeedbackMarkdown: "## Test\n\nConsistent output test.",
            SkillName: "TestSkill",
            Timestamp: new DateTime(2025, 1, 1, 12, 0, 0)
        );

        // Act
        var result1 = _pdfService.GenerateFeedbackPdf(request);
        var result2 = _pdfService.GenerateFeedbackPdf(request);

        // Assert
        result1.Length.Should().Be(result2.Length);
    }
}
