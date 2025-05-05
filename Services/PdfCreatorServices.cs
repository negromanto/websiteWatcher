using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace websiteWatcher.Services;

public class PdfCreatorServices(ILogger<PdfCreatorServices> logger)
{
    public async Task<Stream> ConvertPageToPdfAsync(string url)
    {
        try
        {
            var browserFetcher = new BrowserFetcher();

            await browserFetcher.DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            await using var page = await browser.NewPageAsync();
            await page.GoToAsync(url);
            await page.EvaluateExpressionHandleAsync("document.fonts.ready");
            var result = await page.PdfStreamAsync();
            result.Position = 0;

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error al crear el PDF: {ex.Message}");
            throw;
        }

        //return Stream.Null;

    }
}
