using Markdig.Extensions.MediaLinks;
using Markdig.Parsers;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Renderers.Html;

namespace Timesheet.Common
{
    public class MarkdownRenderer
    {
        private readonly MarkdownPipeline pipeline;
        public MarkdownRenderer()
        {
            pipeline = CreateMarkdownPipeline();
        }

        public string Render(string markdown, bool absolute)
        {
            var writer = new StringWriter();
            var renderer = new HtmlRenderer(writer);
            if (absolute) renderer.BaseUrl = new Uri("https://markheath.net");
            pipeline.Setup(renderer);

            var document = MarkdownParser.Parse(markdown, pipeline);
            renderer.Render(document);
            writer.Flush();

            return writer.ToString();
        }

        private static MarkdownPipeline CreateMarkdownPipeline()
        {
            var builder = new MarkdownPipelineBuilder()
                .UseYamlFrontMatter()
                .UseCustomContainers()
                .UseEmphasisExtras()
                .UseGridTables()
                .UseMediaLinks()
                .UsePipeTables()
                .UseGenericAttributes(); // Must be last as it is one parser that is modifying other parsers

            var me = builder.Extensions.OfType<MediaLinkExtension>().Single();
            me.Options.ExtensionToMimeType[".mp3"] = "audio/mpeg"; // was missing (should be in the latest version now though)
            builder.DocumentProcessed += document => {
                foreach (var node in document.Descendants())
                {
                    if (node is Markdig.Syntax.Block)
                    {
                        if (node is Markdig.Extensions.Tables.Table)
                        {
                            node.GetAttributes().AddClass("md-table");
                        }
                    }
                }
            };
            return builder.Build();
        }
    }
}
