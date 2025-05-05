using System;
using System.Runtime.CompilerServices;
using Azure.Storage.Blobs;
using HtmlAgilityPack;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using websiteWatcher.Services;

namespace websiteWatcher.Functions;

public class Watcher(ILogger<Watcher> logger, PdfCreatorServices pdfCreatorServices)
{
    private const string InputQuery = @"Select w.Id, w.Url, w.XPathExpression, s.Content AS LatestContent
                                FROM dbo.Websites w
                                LEFT JOIN dbo.Snapshots s ON w.Id = s.Id
                                WHERE s.Timestamp = (select MAX(Timestamp) from dbo.Snapshots WHERE Id = w.Id)";

    [Function(nameof(Watcher))]
    [SqlOutput("dbo.Snapshots", "WebsitesWatcher")]
    public async Task<SnapshotRecord?> Run([TimerTrigger(" */20 * * * * *")] TimerInfo myTimer,
        [SqlInput(InputQuery, "WebsitesWatcher")] IReadOnlyList<WebsiteModel> websites)
    {
        logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

        SnapshotRecord? result = null;

        foreach (var website in websites)
        {
            HtmlWeb web = new();
            HtmlDocument doc = web.Load(website.Url);
            var divWithContent = doc.DocumentNode.SelectSingleNode(website.XPathExpression);

            var content = divWithContent != null ? divWithContent.InnerText.Trim() : "Sin contenido";

            var contectHasChanged = content != website.LatestContent;

            if (contectHasChanged)
            {
                logger.LogInformation($"El contenido a cambiado");

                try
                {

                    var newPdf = await pdfCreatorServices.ConvertPageToPdfAsync(website.Url);

                    logger.LogInformation($"El pdf a sido creado en memoria y esta listo para guardarse");

                    var connString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

                    logger.LogInformation($"La variable de entorno para AzureWebJobsStorage se obtubo correctamente");

                    var blobClient = new BlobClient(connString, "pdfs", $"{website.Id}-{DateTime.UtcNow:yyyyMMddhhmmss}.pdf");
                    await blobClient.UploadAsync(newPdf);

                    logger.LogInformation("El nuevo PDF ha sido creado");

                    result = new SnapshotRecord(website.Id, content);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error al crear el PDF: {ex.Message}");
                }

            }
        }

        return result;
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




public class WebsiteModel 
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public string XPathExpression { get; set; }
    public string LatestContent { get; set; }
}

