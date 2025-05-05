using HtmlAgilityPack;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace websiteWatcher.Functions;

public class Snapshot(ILogger<Snapshot> logger)
{

    [Function(nameof(Snapshot))]
    [SqlOutput("dbo.Snapshots", "WebsitesWatcher")]
    public SnapshotRecord? Run(
            [SqlTrigger("dbo.Websites", "WebsitesWatcher")] 
            IReadOnlyList<SqlChange<Website>> changes)
    {
        //logger.LogInformation("SQL Changes: " + JsonConvert.SerializeObject(changes));

        SnapshotRecord? result = null;

        foreach (var change in changes)
        {
            logger.LogInformation($"Operation: {change.Operation}");
            logger.LogInformation($"Id: {change.Item.Id}, Url: {change.Item.Url}");

            if (change.Operation != SqlChangeOperation.Insert)
            {
                continue;
            }

            HtmlWeb web = new();
            HtmlDocument doc = web.Load(change.Item.Url);
            var divWithContent = doc.DocumentNode.SelectSingleNode(change.Item.XPathExpression);

            var content = divWithContent != null ? divWithContent.InnerText.Trim() : "Sin contenido";

            logger.LogInformation(content);

            result = new SnapshotRecord(change.Item.Id, content);

        }

        return result;
    }
}

public record SnapshotRecord(Guid Id, string Content);

