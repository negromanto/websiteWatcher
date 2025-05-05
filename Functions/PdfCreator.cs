using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PuppeteerSharp;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using websiteWatcher.Services;

namespace websiteWatcher.Functions;

public class PdfCreator(ILogger<PdfCreator> logger, PdfCreatorServices pdfCreatorServices)
{

    // Visit https://aka.ms/sqltrigger to learn how to use this trigger binding
    //[Function(nameof(PdfCreator))]
    //[BlobOutput("pdfs/new.pdf", Connection = "WebsitesWatcherStorage")]
    //public async Task<byte[]?> Run(
    //    [SqlTrigger("dbo.Websites", "WebsitesWatcher")] SqlChange<Website>[] changes)
    //{
    //    //logger.LogInformation("SQL Changes: " + JsonConvert.SerializeObject(changes));

    //    byte[]? buffer = null;

    //    foreach (var change in changes)
    //    {
    //        if (change.Operation == SqlChangeOperation.Insert)
    //        {
    //           var result = await ConvertPageToPdfAsync(change.Item.Url);
    //            logger.LogInformation($"El length del PDF es: {result.Length}");

    //            buffer = new byte[result.Length];
    //            await result.ReadAsync(buffer);
    //        }
    //    }

    //    return buffer;
    //}


    [Function(nameof(PdfCreator))]

    public async Task Run(
        [SqlTrigger("dbo.Websites", "WebsitesWatcher")] SqlChange<Website>[] changes)
    {
        //logger.LogInformation("SQL Changes: " + JsonConvert.SerializeObject(changes));

        
        foreach (var change in changes)
        {
            if (change.Operation == SqlChangeOperation.Insert)
            {
                var result = await pdfCreatorServices.ConvertPageToPdfAsync(change.Item.Url);
                logger.LogInformation($"El length del PDF es: {result.Length}");

                //var connString = Environment.GetEnvironmentVariable("ConnectionStrings:WebsitesWatcherStorage");
                var connString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                var blobClient = new BlobClient(connString, "pdfs", $"{change.Item.Id}.pdf");
                await blobClient.UploadAsync(result);

            }
        }

    }



    //private async Task<Stream> ConvertPageToPdfAsync(string url)
    //{
    //    var browserFetcher = new BrowserFetcher();

    //    await browserFetcher.DownloadAsync();
    //    await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
    //    await using var page = await browser.NewPageAsync();
    //    await page.GoToAsync(url);
    //    await page.EvaluateExpressionHandleAsync("document.fonts.ready");
    //    var result = await page.PdfStreamAsync();
    //    result.Position = 0;

    //    return result;

    //}
}


