using PuppeteerSharp;

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
            return await page.PdfStreamAsync();
        }
    }
}
