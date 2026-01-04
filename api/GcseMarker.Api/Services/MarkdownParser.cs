using System.Text.RegularExpressions;

namespace GcseMarker.Api.Services;

public enum BlockType
{
    Heading1,
    Heading2,
    Heading3,
    Paragraph,
    BulletList,
    HorizontalRule
}

public record ContentBlock(BlockType Type, string Content, List<string>? ListItems = null);

public record TextSpan(string Text, bool IsBold, bool IsItalic);

public interface IMarkdownParser
{
    List<ContentBlock> Parse(string markdown);
    List<TextSpan> ParseInlineFormatting(string text);
}

public class MarkdownParser : IMarkdownParser
{
    public List<ContentBlock> Parse(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
            return new List<ContentBlock>();

        var blocks = new List<ContentBlock>();
        var lines = markdown.Split('\n');
        var currentListItems = new List<string>();
        var currentParagraph = new List<string>();

        foreach (var rawLine in lines)
        {
            var line = rawLine.TrimEnd('\r');

            // Flush paragraph if we hit a special line
            if (IsSpecialLine(line) && currentParagraph.Count > 0)
            {
                blocks.Add(new ContentBlock(BlockType.Paragraph, string.Join(" ", currentParagraph).Trim()));
                currentParagraph.Clear();
            }

            // Flush list if we hit a non-list line
            if (!line.TrimStart().StartsWith("- ") && currentListItems.Count > 0)
            {
                blocks.Add(new ContentBlock(BlockType.BulletList, "", currentListItems.ToList()));
                currentListItems.Clear();
            }

            // Horizontal rule
            if (line.Trim() == "---" || line.Trim() == "***" || line.Trim() == "___")
            {
                blocks.Add(new ContentBlock(BlockType.HorizontalRule, ""));
                continue;
            }

            // Heading 1
            if (line.StartsWith("# "))
            {
                blocks.Add(new ContentBlock(BlockType.Heading1, line[2..].Trim()));
                continue;
            }

            // Heading 2
            if (line.StartsWith("## "))
            {
                blocks.Add(new ContentBlock(BlockType.Heading2, line[3..].Trim()));
                continue;
            }

            // Heading 3
            if (line.StartsWith("### "))
            {
                blocks.Add(new ContentBlock(BlockType.Heading3, line[4..].Trim()));
                continue;
            }

            // Bullet list item
            if (line.TrimStart().StartsWith("- "))
            {
                var itemText = line.TrimStart()[2..].Trim();
                currentListItems.Add(itemText);
                continue;
            }

            // Empty line - flush paragraph
            if (string.IsNullOrWhiteSpace(line))
            {
                if (currentParagraph.Count > 0)
                {
                    blocks.Add(new ContentBlock(BlockType.Paragraph, string.Join(" ", currentParagraph).Trim()));
                    currentParagraph.Clear();
                }
                continue;
            }

            // Regular text - accumulate paragraph
            currentParagraph.Add(line.Trim());
        }

        // Flush remaining content
        if (currentListItems.Count > 0)
        {
            blocks.Add(new ContentBlock(BlockType.BulletList, "", currentListItems.ToList()));
        }

        if (currentParagraph.Count > 0)
        {
            blocks.Add(new ContentBlock(BlockType.Paragraph, string.Join(" ", currentParagraph).Trim()));
        }

        return blocks;
    }

    public List<TextSpan> ParseInlineFormatting(string text)
    {
        if (string.IsNullOrEmpty(text))
            return new List<TextSpan> { new TextSpan("", false, false) };

        var spans = new List<TextSpan>();
        var pattern = @"(\*\*\*(.+?)\*\*\*|\*\*(.+?)\*\*|\*(.+?)\*)";
        var regex = new Regex(pattern);
        var lastIndex = 0;

        foreach (Match match in regex.Matches(text))
        {
            // Add text before match
            if (match.Index > lastIndex)
            {
                spans.Add(new TextSpan(text[lastIndex..match.Index], false, false));
            }

            // Bold and italic (***text***)
            if (match.Groups[2].Success)
            {
                spans.Add(new TextSpan(match.Groups[2].Value, true, true));
            }
            // Bold (**text**)
            else if (match.Groups[3].Success)
            {
                spans.Add(new TextSpan(match.Groups[3].Value, true, false));
            }
            // Italic (*text*)
            else if (match.Groups[4].Success)
            {
                spans.Add(new TextSpan(match.Groups[4].Value, false, true));
            }

            lastIndex = match.Index + match.Length;
        }

        // Add remaining text
        if (lastIndex < text.Length)
        {
            spans.Add(new TextSpan(text[lastIndex..], false, false));
        }

        // If no spans were added, return the original text
        if (spans.Count == 0)
        {
            spans.Add(new TextSpan(text, false, false));
        }

        return spans;
    }

    private static bool IsSpecialLine(string line)
    {
        return line.StartsWith("# ") ||
               line.StartsWith("## ") ||
               line.StartsWith("### ") ||
               line.Trim() == "---" ||
               line.Trim() == "***" ||
               line.Trim() == "___" ||
               line.TrimStart().StartsWith("- ");
    }
}
