using GcseMarker.Api.Models.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GcseMarker.Api.Services;

public class PdfService : IPdfService
{
    private readonly IMarkdownParser _markdownParser;

    // Colors matching the web app
    private static readonly string PrimaryBlue = "#1e40af";
    private static readonly string TextDark = "#111827";
    private static readonly string TextMedium = "#374151";
    private static readonly string TextLight = "#4b5563";
    private static readonly string BorderGray = "#e5e7eb";

    public PdfService(IMarkdownParser markdownParser)
    {
        _markdownParser = markdownParser;
    }

    public byte[] GenerateFeedbackPdf(FeedbackPdfRequest request)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.MarginVertical(40);
                page.MarginHorizontal(50);
                page.DefaultTextStyle(x => x.FontSize(11).FontColor(TextMedium));

                page.Header().Element(c => ComposeHeader(c));
                page.Content().Element(c => ComposeContent(c, request));
                page.Footer().Element(c => ComposeFooter(c, request.Timestamp));
            });
        });

        return document.GeneratePdf();
    }

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(col =>
            {
                col.Item().Text("GCSE Essay Marker")
                    .FontSize(16)
                    .Bold()
                    .FontColor(PrimaryBlue);
            });
        });
    }

    private void ComposeContent(IContainer container, FeedbackPdfRequest request)
    {
        container.Column(column =>
        {
            column.Spacing(10);

            // Grade Band Section
            column.Item().PaddingVertical(15).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Overall Grade Band")
                        .FontSize(12)
                        .FontColor(TextLight);
                    col.Item().Text(request.GradeBand)
                        .FontSize(32)
                        .Bold()
                        .FontColor(PrimaryBlue);
                });

                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Skill")
                        .FontSize(12)
                        .FontColor(TextLight);
                    col.Item().Text(request.SkillName)
                        .FontSize(14)
                        .SemiBold()
                        .FontColor(TextDark);
                });
            });

            // Question Section
            if (!string.IsNullOrWhiteSpace(request.Question))
            {
                column.Item()
                    .Background("#fffbeb")
                    .Border(1)
                    .BorderColor("#fcd34d")
                    .Padding(12)
                    .Column(col =>
                    {
                        col.Item().Text("Essay Question")
                            .FontSize(11)
                            .SemiBold()
                            .FontColor("#92400e");
                        col.Item().PaddingTop(5).Text($"\"{request.Question}\"")
                            .FontSize(11)
                            .Italic()
                            .FontColor(TextDark);
                    });
            }

            // Separator
            column.Item().PaddingVertical(10).LineHorizontal(1).LineColor(BorderGray);

            // Feedback Content
            var blocks = _markdownParser.Parse(request.FeedbackMarkdown);
            RenderBlocks(column, blocks);
        });
    }

    private void RenderBlocks(ColumnDescriptor column, List<ContentBlock> blocks)
    {
        foreach (var block in blocks)
        {
            switch (block.Type)
            {
                case BlockType.Heading1:
                    column.Item().PaddingTop(15).PaddingBottom(8)
                        .Text(block.Content)
                        .FontSize(18)
                        .Bold()
                        .FontColor(TextDark);
                    break;

                case BlockType.Heading2:
                    column.Item().PaddingTop(12).PaddingBottom(6)
                        .Text(block.Content)
                        .FontSize(14)
                        .Bold()
                        .FontColor(TextDark);
                    break;

                case BlockType.Heading3:
                    column.Item().PaddingTop(10).PaddingBottom(4)
                        .Text(block.Content)
                        .FontSize(12)
                        .SemiBold()
                        .FontColor(TextMedium);
                    break;

                case BlockType.Paragraph:
                    column.Item().PaddingBottom(8).Text(text =>
                    {
                        var spans = _markdownParser.ParseInlineFormatting(block.Content);
                        foreach (var span in spans)
                        {
                            var spanText = text.Span(span.Text);
                            if (span.IsBold) spanText = spanText.Bold();
                            if (span.IsItalic) spanText = spanText.Italic();
                        }
                    });
                    break;

                case BlockType.BulletList:
                    if (block.ListItems != null)
                    {
                        column.Item().PaddingLeft(15).PaddingBottom(8).Column(listCol =>
                        {
                            foreach (var item in block.ListItems)
                            {
                                listCol.Item().Row(row =>
                                {
                                    row.ConstantItem(15).Text("•").FontColor(TextMedium);
                                    row.RelativeItem().Text(text =>
                                    {
                                        var spans = _markdownParser.ParseInlineFormatting(item);
                                        foreach (var span in spans)
                                        {
                                            var spanText = text.Span(span.Text);
                                            if (span.IsBold) spanText = spanText.Bold();
                                            if (span.IsItalic) spanText = spanText.Italic();
                                        }
                                    });
                                });
                            }
                        });
                    }
                    break;

                case BlockType.HorizontalRule:
                    column.Item().PaddingVertical(10).LineHorizontal(1).LineColor(BorderGray);
                    break;
            }
        }
    }

    private void ComposeFooter(IContainer container, DateTime timestamp)
    {
        container.Row(row =>
        {
            row.RelativeItem().Text(text =>
            {
                text.Span("Generated: ").FontSize(9).FontColor(TextLight);
                text.Span(timestamp.ToString("dd MMM yyyy HH:mm")).FontSize(9).FontColor(TextLight);
            });

            row.RelativeItem().AlignRight().Text(text =>
            {
                text.Span("Page ").FontSize(9).FontColor(TextLight);
                text.CurrentPageNumber().FontSize(9).FontColor(TextLight);
                text.Span(" of ").FontSize(9).FontColor(TextLight);
                text.TotalPages().FontSize(9).FontColor(TextLight);
            });
        });
    }
}
