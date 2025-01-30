using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Timesheet.Services
{
    public class PdfRendererService
    {
        private IBrowser? _browser = null;

        private async Task<IBrowser> GetBrowser()
        {
            if (_browser == null)
            {
                try
                {
                    await new BrowserFetcher().DownloadAsync();
                    _browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return _browser;
        }



        public async Task<Stream> RenderPdfFromHtml(string html)
        {
            using var browser = await GetBrowser();
            using var page = await browser.NewPageAsync();
            await page.SetContentAsync(html);
            return await page.PdfStreamAsync(new PdfOptions
            {
                Outline = true,
                Format = PaperFormat.A4,
                MarginOptions = new MarginOptions
                {
                    Top = "20px",
                    Right = "20px",
                    Bottom = "40px",
                    Left = "20px"
                },
                DisplayHeaderFooter = true,
                FooterTemplate = "<div id=\"footer-template\" style=\"font-size:10px !important; color:#808080; padding-left:10px\">Bunkai ideas by &copy; Alexander Neumann</div>"
            });
        }
    }
}
