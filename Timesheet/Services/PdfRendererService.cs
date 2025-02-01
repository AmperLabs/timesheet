using PuppeteerSharp;
using PuppeteerSharp.BrowserData;
using PuppeteerSharp.Media;
using System.Runtime.InteropServices;

namespace Timesheet.Services
{
    public class PdfRendererService
    {
        private IBrowser? _browser = null;

        private async Task<IBrowser> GetBrowser()
        {
            // https://g3rv4.com/2022/04/creating-pdfs-on-csharp-in-docker

            if (_browser == null)
            {
                try
                {
                    var launchOptions = new LaunchOptions { Headless = true };

                    var executable = Environment.GetEnvironmentVariable("PUPPETEER_EXECUTABLE_PATH");

                    if(string.IsNullOrEmpty(executable))
                    {
                        var fetcher = new BrowserFetcher();
                        var installedBrowser = await fetcher.DownloadAsync();
                    }
                    else
                    {
                        launchOptions.ExecutablePath = executable;
                        launchOptions.Args = new[] { "--no-sandbox" };
                    }

                    _browser = await Puppeteer.LaunchAsync(launchOptions);
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
