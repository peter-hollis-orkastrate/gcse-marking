using FluentAssertions;
using GcseMarker.Api.Services;

namespace GcseMarker.Api.Tests.Services;

public class MarkdownParserTests
{
    private readonly MarkdownParser _parser;

    public MarkdownParserTests()
    {
        _parser = new MarkdownParser();
    }

    [Fact]
    public void Parse_ReturnsEmptyList_WhenInputIsNull()
    {
        // Act
        var result = _parser.Parse(null!);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Parse_ReturnsEmptyList_WhenInputIsWhitespace()
    {
        // Act
        var result = _parser.Parse("   ");

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void Parse_ParsesHeading1_Correctly()
    {
        // Arrange
        var markdown = "# Main Title";

        // Act
        var result = _parser.Parse(markdown);

        // Assert
        result.Should().HaveCount(1);
        result[0].Type.Should().Be(BlockType.Heading1);
        result[0].Content.Should().Be("Main Title");
    }

    [Fact]
    public void Parse_ParsesHeading2_Correctly()
    {
        // Arrange
        var markdown = "## Section Title";

        // Act
        var result = _parser.Parse(markdown);

        // Assert
        result.Should().HaveCount(1);
        result[0].Type.Should().Be(BlockType.Heading2);
        result[0].Content.Should().Be("Section Title");
    }

    [Fact]
    public void Parse_ParsesHeading3_Correctly()
    {
        // Arrange
        var markdown = "### Subsection Title";

        // Act
        var result = _parser.Parse(markdown);

        // Assert
        result.Should().HaveCount(1);
        result[0].Type.Should().Be(BlockType.Heading3);
        result[0].Content.Should().Be("Subsection Title");
    }

    [Fact]
    public void Parse_ParsesHorizontalRule_WithDashes()
    {
        // Arrange
        var markdown = "---";

        // Act
        var result = _parser.Parse(markdown);

        // Assert
        result.Should().HaveCount(1);
        result[0].Type.Should().Be(BlockType.HorizontalRule);
    }

    [Fact]
    public void Parse_ParsesHorizontalRule_WithAsterisks()
    {
        // Arrange
        var markdown = "***";

        // Act
        var result = _parser.Parse(markdown);

        // Assert
        result.Should().HaveCount(1);
        result[0].Type.Should().Be(BlockType.HorizontalRule);
    }

    [Fact]
    public void Parse_ParsesParagraph_SingleLine()
    {
        // Arrange
        var markdown = "This is a paragraph.";

        // Act
        var result = _parser.Parse(markdown);

        // Assert
        result.Should().HaveCount(1);
        result[0].Type.Should().Be(BlockType.Paragraph);
        result[0].Content.Should().Be("This is a paragraph.");
    }

    [Fact]
    public void Parse_ParsesParagraph_MultipleLines()
    {
        // Arrange
        var markdown = "Line one\nLine two\nLine three";

        // Act
        var result = _parser.Parse(markdown);

        // Assert
        result.Should().HaveCount(1);
        result[0].Type.Should().Be(BlockType.Paragraph);
        result[0].Content.Should().Be("Line one Line two Line three");
    }

    [Fact]
    public void Parse_ParsesBulletList_SingleItem()
    {
        // Arrange
        var markdown = "- First item";

        // Act
        var result = _parser.Parse(markdown);

        // Assert
        result.Should().HaveCount(1);
        result[0].Type.Should().Be(BlockType.BulletList);
        result[0].ListItems.Should().HaveCount(1);
        result[0].ListItems![0].Should().Be("First item");
    }

    [Fact]
    public void Parse_ParsesBulletList_MultipleItems()
    {
        // Arrange
        var markdown = "- First item\n- Second item\n- Third item";

        // Act
        var result = _parser.Parse(markdown);

        // Assert
        result.Should().HaveCount(1);
        result[0].Type.Should().Be(BlockType.BulletList);
        result[0].ListItems.Should().HaveCount(3);
        result[0].ListItems![0].Should().Be("First item");
        result[0].ListItems![1].Should().Be("Second item");
        result[0].ListItems![2].Should().Be("Third item");
    }

    [Fact]
    public void Parse_SeparatesParagraphs_ByBlankLines()
    {
        // Arrange
        var markdown = "First paragraph.\n\nSecond paragraph.";

        // Act
        var result = _parser.Parse(markdown);

        // Assert
        result.Should().HaveCount(2);
        result[0].Type.Should().Be(BlockType.Paragraph);
        result[0].Content.Should().Be("First paragraph.");
        result[1].Type.Should().Be(BlockType.Paragraph);
        result[1].Content.Should().Be("Second paragraph.");
    }

    [Fact]
    public void Parse_HandlesMixedContent()
    {
        // Arrange
        var markdown = "# Title\n\nSome intro text.\n\n## Section\n\n- Point one\n- Point two\n\n---\n\nConclusion.";

        // Act
        var result = _parser.Parse(markdown);

        // Assert
        result.Should().HaveCount(6);
        result[0].Type.Should().Be(BlockType.Heading1);
        result[0].Content.Should().Be("Title");
        result[1].Type.Should().Be(BlockType.Paragraph);
        result[1].Content.Should().Be("Some intro text.");
        result[2].Type.Should().Be(BlockType.Heading2);
        result[2].Content.Should().Be("Section");
        result[3].Type.Should().Be(BlockType.BulletList);
        result[3].ListItems.Should().HaveCount(2);
        result[4].Type.Should().Be(BlockType.HorizontalRule);
        result[5].Type.Should().Be(BlockType.Paragraph);
        result[5].Content.Should().Be("Conclusion.");
    }

    [Fact]
    public void ParseInlineFormatting_ReturnsPlainText_WhenNoFormatting()
    {
        // Arrange
        var text = "Plain text without formatting";

        // Act
        var result = _parser.ParseInlineFormatting(text);

        // Assert
        result.Should().HaveCount(1);
        result[0].Text.Should().Be("Plain text without formatting");
        result[0].IsBold.Should().BeFalse();
        result[0].IsItalic.Should().BeFalse();
    }

    [Fact]
    public void ParseInlineFormatting_ParsesBoldText()
    {
        // Arrange
        var text = "This is **bold** text";

        // Act
        var result = _parser.ParseInlineFormatting(text);

        // Assert
        result.Should().HaveCount(3);
        result[0].Text.Should().Be("This is ");
        result[0].IsBold.Should().BeFalse();
        result[1].Text.Should().Be("bold");
        result[1].IsBold.Should().BeTrue();
        result[1].IsItalic.Should().BeFalse();
        result[2].Text.Should().Be(" text");
    }

    [Fact]
    public void ParseInlineFormatting_ParsesItalicText()
    {
        // Arrange
        var text = "This is *italic* text";

        // Act
        var result = _parser.ParseInlineFormatting(text);

        // Assert
        result.Should().HaveCount(3);
        result[0].Text.Should().Be("This is ");
        result[1].Text.Should().Be("italic");
        result[1].IsItalic.Should().BeTrue();
        result[1].IsBold.Should().BeFalse();
        result[2].Text.Should().Be(" text");
    }

    [Fact]
    public void ParseInlineFormatting_ParsesBoldAndItalicText()
    {
        // Arrange
        var text = "This is ***bold and italic*** text";

        // Act
        var result = _parser.ParseInlineFormatting(text);

        // Assert
        result.Should().HaveCount(3);
        result[0].Text.Should().Be("This is ");
        result[1].Text.Should().Be("bold and italic");
        result[1].IsBold.Should().BeTrue();
        result[1].IsItalic.Should().BeTrue();
        result[2].Text.Should().Be(" text");
    }

    [Fact]
    public void ParseInlineFormatting_HandlesMultipleFormattedSections()
    {
        // Arrange
        var text = "**Bold** and *italic* here";

        // Act
        var result = _parser.ParseInlineFormatting(text);

        // Assert
        result.Should().HaveCount(4);
        result[0].Text.Should().Be("Bold");
        result[0].IsBold.Should().BeTrue();
        result[1].Text.Should().Be(" and ");
        result[2].Text.Should().Be("italic");
        result[2].IsItalic.Should().BeTrue();
        result[3].Text.Should().Be(" here");
    }

    [Fact]
    public void ParseInlineFormatting_ReturnsEmptySpan_WhenInputIsEmpty()
    {
        // Act
        var result = _parser.ParseInlineFormatting("");

        // Assert
        result.Should().HaveCount(1);
        result[0].Text.Should().Be("");
    }
}
